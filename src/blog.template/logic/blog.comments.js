/**
 * powers the comments feature
 */
(enableComments = async () => {

    /**
     * Returns the current users name, or null if not logged in.
     */
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


    /**
     * Returns a comment formatted as Html
     * @param {guid} commentId - Unique id assigned to comment
     * @param {string} comment - Value of comment
     * @param {string} username - Comment posted by
     * @param {date} posted - Current date and time
     */
    const convertCommentToHtml = (commentId, comment, username, posted) => `
        <hr>
        <div id="${commentId}" class="text-muted">
            <strong class="text-primary">&gt;</strong>
            <span>${username}</span>
            <small>
                <div>posted ${posted}</div>
            </small>
            <p>
                    ${comment}
            </p>
        </div>
    `;


    /**
     * Adds a comment to the comments section
     * @param {guid} commentId - Unique id assigned to comment
     * @param {string} comment - Value of comment
     * @param {string} username - Comment posted by
     * @param {date} posted - Current date and time
     */
    const addComment = (commentId, comment, username, posted) => {

        const commentArea = document.querySelector('#comment-area');
        commentArea.innerHTML += convertCommentToHtml(commentId, comment, username, posted);
    }


    /**
     * Downloads and displays all comments linked to the current article
     * @param {guid} id - Article id
     */
    const refreshCommentsSection = async (id) => {

        const commentRequest = await fetch(`/api/v1/comments/${id}`, { method: 'GET' });
        if(commentRequest.ok) {
            const commentJson = await commentRequest.json();

            if('article' in commentJson) {
                if('comments' in commentJson.article) {

                    commentJson.article.comments.forEach(c => {

                        addComment(c.commentId, c.comment, c.username, c.posted);

                    });
                }
            }
        }

        document.querySelector('#comment-area-spinner').remove();
    };


    const id = document.querySelector('head[data-article-id').getAttribute('data-article-id');
    const username = await getUsername();

    refreshCommentsSection(id);

    // Update site based on logged in/out state
    // If a user logs in or out this will trigger a page refresh, therefore no need to handle state change
    if(username) {
        document.querySelector('#comment-login').classList.add('no-show');
        document.querySelector('#comment-username').innerText = username;
        document.querySelector('#comment-add-comment-button').addEventListener('click', async () => {

            $('#comment-model').modal('hide');

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
            document.querySelector('footer').scrollIntoView(false);
        });
    }

    if( ! username ) {
      document.querySelector('#comment-logout').classList.add('no-show');
      document.querySelector('.comment-add-comment').classList.add('no-show');
    }

})();
