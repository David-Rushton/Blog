import * as comments from './modules/comments.js';
import * as upvotes from './modules/upvotes.js';


document.querySelector('#article-up-vote').addEventListener('click', async () => upvotes.incrementUpvoteScore() );
document.querySelector('#comment-add-comment-button').addEventListener('click', async () => comments.addComment() );
