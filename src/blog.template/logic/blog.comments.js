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

    const getComment = (commentId, comment, username, posted) => `
        <hr>
        <div id="${commentId}" class="text-muted">
            <strong class="text-primary">&gt;</strong> <span>${username}</span>
            <small><div>posted ${posted}</div></small>
            <p>
                ${comment}
            </p>
        </div>
    `;

    const addComment = (commentId, comment, username, posted) => {

        const commentArea = document.querySelector('#comment-area');
        commentArea.innerHTML += getComment(commentId, comment, username, posted);
    }


    const id = document.querySelector('head[data-article-id').getAttribute('data-article-id');
    const username = await getUsername();

    if(username) {
        document.querySelector('#comment-login').classList.add('no-show');
        document.querySelector('#comment-username').innerText = username;
        document.querySelector('#comment-add-comment-button').addEventListener('click', async () => {

            const commentTag = document.querySelector('#comment-text');

            if(commentTag.value == '' || commentTag.value == null || commentTag.value == undefined)
                return;

            const comment = {
                parentCommentId: id,
                username: username,
                comment: commentTag.value
            };

            addComment('000-000', comment.comment, comment.username, new Date().toLocaleString());

            await fetch(`/api/v1/comments/${id}`, {
                method: 'POST',
                header: { 'Content-Type': 'application/json'},
                body: JSON.stringify(comment)
            });


            commentTag.value = '';
            $('#comment-model').modal('hide');
        });
    }

    if( ! username ) {
      document.querySelector('#comment-logout').classList.add('no-show');
      document.querySelector('.comment-add-comment').classList.add('no-show');
    }







})();
