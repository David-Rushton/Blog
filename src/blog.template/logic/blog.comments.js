/**
 * powers the comments feature
 */
(enableComments = async () => {

  const getUsername = async () => {

    const isLocalEnv = window.location.href.startsWith('http://127.0.0.1:');

    if(isLocalEnv)
      return "test-user";


      const authResponse = await fetch('/.auth/me');
      if( ! authResponse.ok ) {
          console.error('Cannot retrieve login details');
          return;
      }

      const authJson = await authResponse.json();
      const { clientPrincipal } = authJson;
      const loggedIn = (clientPrincipal != null);

      return loggedIn ? clientPrincipal.userDetails : null;
  };


  const username = await getUsername();

    if(username) {
        document.querySelector('#comment-login').classList.add('no-show');
        document.querySelector('#comment-username').innerText = username;
    }

    if( ! username ) {
      document.querySelector('#comment-logout').classList.add('no-show');
      document.querySelector('.comment-add-comment').classList.add('no-show');
    }

})();



/*
{
    "clientPrincipal": {
      "identityProvider": "github",
      "userId": "aa5052fdfe4c469ea4a0de6192fe1c98",
      "userDetails": "David-Rushton",
      "userRoles": [
        "admin",
        "anonymous",
        "authenticated"
      ]
    }
  }
*/
