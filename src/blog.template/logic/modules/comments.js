let _username = null;
let _loggedIn = null;


/**
 * fetches comments from db and updates UI
 * @param {object[]} comments - array of comments
 */
async function loadComments(comments) {
    try {

        // Sets value of _username and _loggedIn fields
        await getUsername();

        setAuthControlsVisibility();
        initialiseNewCommentDialog();
        loadAndDisplayComments(comments);
    }
    catch(e) {

        throw new commentException(e.message || 'cannot load comments');
    }
    finally {

        document.querySelector('#comment-area-spinner').remove();
    }
}


/**
 * Adds a new comment to the article
 */
async function addNewComment(id) {

    try {

        // Writing to the db takes time.
        // To make the site more responsive and feel faster we write update the UI and then write to
        // the db.

        $('#comment-model').modal('hide');

        const commentTag = document.querySelector('#comment-text');

        if( ! commentTag.value )
            return;

        const commentObj = {
            parentCommentId: id,
            username: _username,
            comment: commentTag.value
        };

        addComment('000-000-000', commentObj.comment, _username, new Date().toLocaleString());

        commentTag.value = '';

        await fetch(`/api/v1/comments/${id}`, {
            method: 'POST',
            header: { 'Content-Type': 'application/json'},
            body: JSON.stringify(commentObj)
        });

        document.querySelector('footer').scrollIntoView(false);
    }
    catch(e) {

        throw new commentException(e.message || 'cannot post new comment');
    }
}


/**
 * controls with auth controls are/are not visible
 * if user logged in the log in button is hidden
 * if not logged in the log out and post comment buttons are hidden
 */
function setAuthControlsVisibility() {

    if(_loggedIn) {
        document.querySelector('#comment-login').classList.add('no-show');
        document.querySelector('.comment-add-comment').classList.remove('no-show');
        document.querySelector('#comment-logout').classList.remove('no-show');
    }
}


/**
 * Injects the current username
 */
function initialiseNewCommentDialog() {

    // no action is required if not logged in.
    // logging in results in a page refresh, meaning we do not need to deal
    // with state transitions.
    if(_loggedIn)
        document.querySelector('#comment-username').innerHTML = _username;
}


/**
 * comments are read from the db and displayed in the UI
 * @param {object[]} comments - array of comments
 */
function loadAndDisplayComments(comments) {

    if(comments) {
        comments.forEach(c => {

            addComment(c.commentId, c.comment, c.username, c.posted);
        });
    }
}


/**
 * returns the current username, or null if not logged in
 */
async function getUsername() {

    if(_username == null) {

    /*
        // TODO: create a local dev env with proxy access to stage/prod auth services.
        // current local dev environments called make a call to the auth API.  Instead we inject a test username.  The
        // username is only used by the client.  The backend api has a separate method for handling auth services.
        let siteUri = window.location.href;
        if(siteUri.startsWith('http://127.0.0.1:') || siteUri.startsWith('http://localhost:')) {
            _username = 'test-user';
            _loggedIn = true;
            return ;
        }
    */


        const authResponse = await fetch('/.auth/me', { method: 'GET', mode: 'no-cors' });

        if( ! authResponse.ok )
            throw new commentException('cannot fetch login details');

        const authJson = await authResponse.json();
        const { clientPrincipal } = authJson;

        _loggedIn = (clientPrincipal != null);
        _username = _loggedIn ? clientPrincipal.userDetails : null;
    }
}


/**
 * Adds a comment to the comments section
 * @param {guid} commentId - Unique id assigned to comment
 * @param {string} comment - Value of comment
 * @param {string} username - Comment posted by
 * @param {date} posted - Current date and time
 */
function addComment(commentId, comment, username, posted) {

    const commentArea = document.querySelector('#comment-area');
    commentArea.innerHTML += convertCommentToHtml(commentId, comment, username, posted);
}


/**
 * Returns a comment formatted as Html
 * @param {guid} commentId - Unique id assigned to comment
 * @param {string} comment - Value of comment
 * @param {string} username - Comment posted by
 * @param {date} posted - Current date and time
 */
function convertCommentToHtml(commentId, comment, username, posted) {
    return `
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
}


/**
 * returns a custom exception
 * @param {int} status - http status code
 * @param {string} message - exception message
 */
function commentException(message) {
    this.message = message;

    this.toString = () => message;
}


export { loadComments, addNewComment };
