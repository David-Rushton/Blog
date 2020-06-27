---
title: Building the Blog
slug: I like to define what I'm trying to achieve
tags: [code, intro, jamstack, ci-cd, sdlc]
date: 2020-06-10
image: /blog.articles/media/chart-close-up-data-desk-590022.jpg
image-credit: Photo by Lukas from Pexels
---

I love a list.  The act of writing them helps me to clarify my thoughts.  Ticking off completed tasks provides a
satisfying sense of achievement.  And of course; organising data is kind of my thing.  So where else could my blog start
but with a list?  At the start of any project I like to bullet out what I'm hoping to achieve.  A good list aids the
process of converting an idea into a plan.  And so here it is:

- I want to build my blog from scratch
- I want to write my articles using [markdown](https://daringfireball.net/projects/markdown/syntax)
- [I want the code I write to look beautiful](./making-code-pop.md)
- I want to keep things simple
- I want it to be fun

I really enjoy building things.  I'm not a web developer but I can sling a little JavaScript.  A while back I came
across the [Jamstack](https://jamstack.org/).  Jamstack isn't a technology, it is an approach to building websites.  The
core idea is you don't need a webserver.  **J**avaScrip and **A**PIs provide all the dynamic flourishes you'll ever need,
while pages (**M**arkup) are prebuilt and deployed to a [content delivery network (CDN)](https://en.wikipedia.org/wiki/Content_delivery_network).
I love the simplicity of this idea.  Server side processes can be replaced with microservices.  Each microservice is
free to focus on one job, and one job alone.  That's the kind of code I like to write.  Focused.  Also, if I'm being
honest, the idea of not paying for backend compute is pretty appealing.

I mentioned simplicity.  What could be simpler than a site that updates itself following every master merge?  Changes to
my blog are automatically built, tested and deployed.  I'm a big fan of [continuous deployment](https://www.redhat.com/en/topics/devops/what-is-ci-cd).
For a silly hobby project like this it is relatively simple to implement.  But on bigger and more complicated projects
moving this fast can be hard to achieve.  It is worth the effort.  Moving faster requires that you automate your
build pipelines.  Developers shouldn't be pressing the same buttons again and again.  The job title implies that our
role is to create new things.  Automating deployments/integrations frees up valuable time to squish bugs and build new
features.  You would have to be very brave to deploy untested code.  Here CI/CD delivers yet more benefits.  Automating
your tests and ensuring you have appropriate coverage frees up more time.  Working on well tested code provides an extra
sense of freedom.  You are free to make changes, knowing any mistakes will be caught as your branch builds.  That
freedom combined with the speed that only automation can reliable provide pays dividends come the day you need to patch
a flaw fast.  Like I said; I'm a big fan.

I'll save the rest the other items for future posts.  I won't forget.  Thanks to my list.
