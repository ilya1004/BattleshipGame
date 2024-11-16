
//let logoutButton = null;

//function addEventToLogoutButton() {
//    logoutButton = document.getElementById("logout-button");
//    console.log("qwe");
//    if (logoutButton != null) {
//        logoutButton.addEventListener("click", async (event) => logoutUser(event))
//    }
//}

//logoutButton = document.getElementById("logout-button");

//if (logoutButton != null) {
//    logoutButton.addEventListener("click", async (event) => logoutUser(event))
//}

//const logoutUser = async (event) => {
//    event.preventDefault();
//    try {
//        const response = await fetch('api/Account/Logout', {
//            method: 'POST',
//            credentials: 'include'
//        });

//        if (response.ok) {
//            window.location.href = '/';
//        } else {
//            alert('Ошибка при выходе. Пожалуйста, попробуйте еще раз.');
//        }
//    } catch (error) {
//        console.error('Ошибка:', error);
//        alert('Произошла ошибка. Пожалуйста, попробуйте еще раз.');
//    }
//}

//function navToMainPage() {
//    window.location.href = '/';
//}