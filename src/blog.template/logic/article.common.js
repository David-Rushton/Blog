import * as article from './modules/article.js';
import * as comments from './modules/comments.js';
import * as upvotes from './modules/upvotes.js';

(async () => {

    const articleDoc = await article.getArticle();
    const articleId = article.getId();


    upvotes.loadUpvoteScore(articleDoc?.article?.upVotes || 1);
    comments.loadComments(articleDoc?.article?.comments);

    document.querySelector('#article-up-vote')
        .addEventListener('click', async () => upvotes.incrementUpvoteScore(articleId))
    ;

    document.querySelector('#comment-add-comment-button')
        .addEventListener('click', async () => comments.addNewComment(articleId))
    ;
})();
