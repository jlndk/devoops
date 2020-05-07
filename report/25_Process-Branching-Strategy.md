# Branching Strategy

Due to the smaller scope and team size, it was decided to use a lightweight, but surprisingly useful, branching strategy called GitHub flow (TODO: ref) (not to be confused with Git flow). GitHub flow works like this:

* There are two types of branches, master and feature branches. The single master branch is always deployable, but, though preferred, feature branches do not need to be.
* Feature branches should have names describing the feature. You can optionally chose to prepend a personal id to the branch name for easy identification, like so: “tobl/table-date-filtering”.
* Push often and keep the branch as small as possible, for easy reviewing and merging.
* Whenever a developer needs to share their work, either for feedback or for help, they create a pull request, possibly as a draft if needed.
* When the feature is ready to be merged and someone else has reviewed it, the branch is merged into master and deleted.
* Master is automatically released and deployed after being updated.

Another reason that GitHub Flow was chosen, was that it is more geared towards continuous releases, since it isn’t based around release branches like Git Flow is. This has been very effective and has allowed the team to focus on delivering quality features as often as possible. One caveat is that the team needs to be quick in reviewing PRs when needed, as to now block other features, that may depend on that new branch.
