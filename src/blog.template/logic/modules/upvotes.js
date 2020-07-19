const _upvoteTag = document.querySelector('#article-up-vote-count');
let _score = 1;


/**
 * fetches the current upvote score form the db and refreshes the display
 * @param {int} upvoteScore - current upvote score
 */
function loadUpvoteScore(upvoteScore) {
    try {
        _score = upvoteScore;
    }
    catch(e) {

        throw new upvoteException(e.message || 'cannot download upvote score');
    }
    finally {

        refreshUpVoteScore(_score);
    }
}


/**
 * updates the displayed upvote score
 * @param {int} score - score to display
 */
function refreshUpVoteScore(score) {

    if( ! _upvoteTag )
        throw new upvoteException('cannot find upvote element');

    _upvoteTag.innerText = score.toLocaleString();
}


/**
 * increments the upvote score
 * @param {string} - id of the article to upvote
 */
async function incrementUpvoteScore(id) {

    refreshUpVoteScore(_score++);

    const response = await fetch(`/api/v1/upvotes/${id}`, { method: method });

    if( ! response.ok ) {
        const responseJson = await response.json();
        throw new upvoteException(responseJson.message || `cannot upvote article: ${id}`);
    }
};


/**
 * returns a custom exception
 * @param {int} status - http status code
 * @param {string} message - exception message
 */
function upvoteException(message) {
    this.message = message;

    this.toString = () => message;
}


export { loadUpvoteScore, incrementUpvoteScore };
