using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Data;
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
        public async Task<IActionResult> MyPage()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var model = new UserViewModel(result);
            model.Friends = await GetAllFriend(model.User);

            return View("User", model);
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
            
            //Типа autoaccept друга
            repository.AddFriend(friend, findUser); 

            return RedirectToAction("MyPage", "AccountManager");
        }

        [Route("DeleteFriend")]
        [HttpPost]
        public async Task<IActionResult> DeleteFriend(string id)
        {
            var currentuser = User;

            var findUser = await _userManager.GetUserAsync(currentuser);

            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;
            
            repository.DeleteFriend(findUser, friend);
            
            //Автовыпил из друзей
            repository.DeleteFriend(friend, findUser);

            return RedirectToAction("MyPage", "AccountManager");
        }

        [Route("Chat")]
        [HttpPost]
        public async Task<IActionResult> Chat(string id)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);
            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            var mess = repository.GetMessages(result, friend);

            var model = new ChatViewModel()
            {
                You = result,
                ToWhom = friend,
                MessagesHistory = mess.OrderBy(x => x.Id).ToList(),
            };
            return View("Chat", model);
        }

        [Route("Chat")]
        [HttpGet]
        public async Task<IActionResult> Chat()
        {

            var id = Request.Query["id"];

            var model = await GenerateChat(id);
            return View("Chat", model);
        }

        [Route("NewMessage")]
        [HttpPost]
        public async Task<IActionResult> NewMessage(string id, ChatViewModel chat)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);
            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            var item = new Message()
            {
                Sender = result,
                Recipient = friend,
                Text = chat.NewMessage.Text,
            };
            repository.Create(item);

            var mess = repository.GetMessages(result, friend);

            var model = new ChatViewModel()
            {
                You = result,
                ToWhom = friend,
                MessagesHistory = mess.OrderBy(x => x.Id).ToList(),
            };
            //Тут проблема с автообновлением страницы при авторефреше после отправки сообщения
            return View("Chat", model);
        }

        //Чиним авторефреш
        [Route("NewMessage")]
        [HttpGet]
        public async Task<IActionResult> NewMessage(string id)
        {
            var model = await GenerateChat(id);
 
            return View("Chat", model);
        }

        [Route("Generate")]
        [HttpGet]
        public async Task<IActionResult> Generate()
        {

            var usergen = new GenetateUsers();
            var userlist = usergen.Populate(35);

            foreach (var user in userlist)
            {
                var result = await _userManager.CreateAsync(user, "12345");

                if (!result.Succeeded)
                    continue;
            }

             return RedirectToAction("Index", "Home");
        }

        #region Library
        private async Task<SearchViewModel> CreateSearch(string search)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);

            //Задание 35.6.6
            var list = _userManager.Users.AsEnumerable().ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
            }

            //var list = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().ToUpper().Contains(search.ToUpper())).ToList();
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

        private async Task<List<User>> GetAllFriend(User user)
        {
            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return repository.GetFriendsByUser(user);
        }

        private async Task<List<User>> GetAllFriend()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return repository.GetFriendsByUser(result);
        }

        private async Task<ChatViewModel> GenerateChat(string id)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);
            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            var mess = repository.GetMessages(result, friend);

            var model = new ChatViewModel()
            {
                You = result,
                ToWhom = friend,
                MessagesHistory = mess.OrderBy(x => x.Id).ToList(),
            };

            return model;
        }

        #endregion #region Library
    }
}
