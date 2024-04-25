using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Data.Repository;
using SocialNetwork.Data.UoW;
using SocialNetwork.Extentions;
using SocialNetwork.Models.Users;
using SocialNetwork.ViewModels.Account;

namespace SocialNetwork.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IUnitOfWork _unitOfWork;

        public AccountManagerController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Home/Login");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = _mapper.Map<User>(model);

                var userFind = await _userManager.FindByEmailAsync(user.Email);

                if (userFind != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(userFind.UserName, model.Password, model.RememberMe, false);
                    //if (result.Succeeded)
                    //{
                    //    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    //    {
                    //        return Redirect(model.ReturnUrl);                            
                    //    }
                    //    else
                    //    {
                    //        //return RedirectToAction("Index", "Home");
                    //        //return View("User", new UserViewModel(userFind));
                    //        return RedirectToAction("MyPage", "AccountManager");
                    //    }
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    //}

                    //Задание 35.4.6
                    if (result.Succeeded)
                    {
                        return RedirectToAction("MyPage", "AccountManager");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    }
                }
            }

            //return View("Views/Home/Index.cshtml", new StoreOfModels());
            //return View("Views/Home/Index.cshtml", model);
            return RedirectToAction("Index", "Home");

        }

        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("MyPage")]
        [HttpGet]
        [Authorize]
        public IActionResult MyPage()
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            return View("User", new UserViewModel(result.Result));
        }

        //Получение данных для редактирования юзера
        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit() //@@@
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            var editUserModel = _mapper.Map<UserEditViewModel>(result.Result);

            return View("Edit", editUserModel);
        }

        //Обновление данных (постим) отредактированного юзера
        [Authorize]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "AccountManager");
                }
                else
                {
                    return RedirectToAction("Edit", "AccountManager");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        [Route("UserList")]
        [HttpPost]
        public async Task<IActionResult> UserList(string search)
        {
            var model = await CreateSearch(search);
            return View("UserList", model);
        }

        [Route("AddFriend")]
        [HttpPost]
        public async Task<IActionResult> AddFriend(string id)
        {
            var currentuser = User;

            var findUser = await _userManager.GetUserAsync(currentuser);

            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            repository.AddFriend(findUser, friend);
            
            return RedirectToAction("MyPage", "AccountManager");
        }

    private async Task<SearchViewModel> CreateSearch(string search)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);

            var list = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().ToUpper().Contains(search.ToUpper())).ToList();
            var withfriend = await GetAllFriend();

            var data = new List<UserWithFriendExt>();
            list.ForEach(x =>
            {
                var t = _mapper.Map<UserWithFriendExt>(x);
                t.IsFriendWithCurrent = withfriend.Where(y => y.Id == x.Id || x.Id == result.Id).Count() != 0;
                data.Add(t);
            });

            var model = new SearchViewModel()
            {
                UserList = data
            };

            return model;
        }

        private async Task<List<User>> GetAllFriend()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return repository.GetFriendsByUser(result);
        }
    }
}
