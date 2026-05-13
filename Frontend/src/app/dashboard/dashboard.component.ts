import { NgFor, NgIf, NgClass, SlicePipe } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../Common/sidebar/sidebar.component';
import { Router, RouterModule } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { PostResponse } from '../../Models/Posts/Responses/PostResponse';
import { DislikeRequest } from '../../Models/Reacts/Requests/DislikeRequest';
import { LikeRequest } from '../../Models/Reacts/Requests/LikeRequest';
import { AddCommentRequest } from '../../Models/Comments/Requests/AddCommentRequest';
import { FollowRequest } from '../../Models/Follow/Requests/FollowRequest';
import { UserResponse } from '../../Models/Users/Responses/UserResponse';
import { LikeCommentRequest } from '../../Models/Comments/Requests/LikeCommentRequest';
import { DislikeCommentRequest } from '../../Models/Comments/Requests/DislikeCommentRequest';
import { EditCommentRequest } from '../../Models/Comments/Requests/EditCommentRequest';
import { CommentResponse } from '../../Models/Posts/Responses/CommentResponse';
import { AddPostRequest } from '../../Models/Posts/Requests/AddPostRequest';
import { EditPostRequest } from '../../Models/Posts/Requests/EditPostRequest';
import { debug } from 'console';
import { UserResponseWithStories } from '../../Models/Users/Responses/UserResponseWithStories';
import { StoryResponse } from '../../Models/Story/Responses/StoryResponse';
import { AddStoryCommentRequest } from '../../Models/Story/Requests/AddStoryCommentRequest';
import { ProfileResponse } from '../../Models/Profiles/Responses/ProfileResponse';
import { UserServiceService } from '../Services/user-service.service';
import { AuthenticationService } from '../Services/authentication-service.service';
import { LikesServiceService } from '../Services/likes-service.service';
import { PostServiceService } from '../Services/post-service.service';
import { StoryServiceService } from '../Services/story-service.service';
import { SharePostServiceService } from '../Services/share-post-service.service';
import { CommentServiceService } from '../Services/comment-service.service';
import { FollowSerivceService } from '../Services/follow-serivce.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [FormsModule, SidebarComponent, RouterModule, NgIf, NgFor, NgClass, SlicePipe,TranslateModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  constructor(private router: Router, private toastService: ToastrService, private userService: UserServiceService, private shareService: SharePostServiceService,
    private likeService: LikesServiceService, private authService: AuthenticationService, private commentService: CommentServiceService,
    private postService: PostServiceService, private storyService: StoryServiceService, private followService: FollowSerivceService) { }

  ngOnInit(): void {
    this.loadPosts();
    this.loadStories();
    this.loadSuggestedUsers();
    this.loadTrendingPosts();
  }
  //****************variables *********************** */

  posts: PostResponse[] = []
  searchQuery: string = '';
  isOpen = false;
  users: any[] = [];
  newStatus = '';
  selectedImage: File | null = null;
  selectedImagePreview: string | null = null;
  stories: StoryResponse[] = [];
  suggestedUsers: UserResponse[] = [];
  friendRequests: ProfileResponse[] = [];
  showRequests = false;
  trendingPosts: PostResponse[] = [];
  userId!: string
  profileId!: string
  newPostText!: string
  newPostTitle!: string
  showCreatePostModal = false;
  currentUserName: string = this.authService.getUserName() ?? '1'
  newPostFeeling = 1;
  newPostImagePreview: string | null = null;
  newPostImageFile: File | null = null;
  storyComment = ''
  selectedUser: any;
  selectedStory: any;
  showCreateStory = false;
  activeStoryUser: any = null;
  openStory(user: any, story: StoryResponse) {
    this.selectedUser = user;
    this.selectedStory = story;
    this.viewStory(story);
  }
  //****************************** functions *************************************** */
  viewProfile(profileId: string) {
    this.router.navigate(['/home/profile', profileId]);
  }

  goToPost(postId: string) {
    this.router.navigate(['/home/user/post', postId]);
  }

  loadTrendingPosts() {
    this.postService.getTrendingPosts().subscribe({
      next: (res: any) => {
        this.trendingPosts = res;
      },
      error: (err) => {
        this.toastService.error(err);
      }
    });
  }
  /////******************************friend requests **********************************/
  acceptRequest(id: string) {

    this.followService.acceptFollow({
      reciever: this.authService.getProfileId() ?? '1',
      sender: id
    }).subscribe(() => {
      this.friendRequests = this.friendRequests.filter(r => r.id !== id);
    });
  }

  rejectRequest(id: string) {
    this.followService.rejectFollow({
      reciever: this.authService.getProfileId() ?? '1',
      sender: id
    }).subscribe(() => {
      this.friendRequests = this.friendRequests.filter(r => r.id !== id);
    });
  }

  toggleRequests() {
    this.showRequests = !this.showRequests;
    this.loadFriendRequests();
  }

  loadFriendRequests() {

    this.followService.viewRequests(this.profileId).subscribe({
      next: (res: ProfileResponse[]) => {
        this.friendRequests = res;
      },
      error: (err) => {
        this.toastService.error(err);
      }
    });
  }

  toggleFollow(user: any) {
    console.log(user)
    const myId = this.authService.getUserId() ?? '';

    const data: FollowRequest = {
      sender: myId,
      reciever: user.id
    };
    if (!user.isRequested) {
      this.followService.requestFollow(data).subscribe({
        next: () => {
          user.isRequested = true;
        }
      });
    } else {
      this.followService.unrequestFollow(data).subscribe({
        next: () => {
          user.isRequested = false;
        }
      });
    }
  }

  loadSuggestedUsers() {
    // debugger
    const userId = this.authService.getUserId() ?? '';

    this.userService.suggestUser(userId).subscribe({
      next: (res: any[]) => {
        this.suggestedUsers = res;
        // console.log(this.suggestedUsers)
      },
      error: (err) => {
        this.toastService.error(err);
      }
    });
  }

  ////////////***************************share******************************************* //
  toggleSharePost(post: any) {
    // debugger
    post.showShareBox = true;
    post.shareLinkLoading = true;

    this.shareService.startSharePost({
      postId: post.id,
      profileId: this.authService.getProfileId() ?? ''
    }).subscribe({
      next: (res: any) => {
        post.shareLink = res;
        post.shareLinkLoading = false;
        console.log(post.shareLink);
      },
      error: () => {
        post.shareLink = 'Error generating link';
        post.shareLinkLoading = false;
      }
    });
  }

  copyToClipboard(link: string) {
    navigator.clipboard.writeText(link);
    this.toastService.success("Link copied!");
  }


  shareToWhatsApp(link: string) {
    const url = `https://wa.me/?text=${encodeURIComponent(link)}`;
    window.open(url, '_blank');
  }
  shareToTwitter(link: string) {
    const text = "Check this post";
    const url = `https://twitter.com/intent/tweet?text=${encodeURIComponent(text)}&url=${encodeURIComponent(link)}`;
    window.open(url, '_blank');
  }

  //************************comments ********************************** */
  loadComments(post: any) {
    // debugger
    if (post.showComments) {
      post.showComments = false
      return;
    }
    this.commentService.getComment(post.id).subscribe({
      next: (res: CommentResponse[]) => {
        post.comments = res
        post.showComments = true
      },
      error: (err) => {
        this.toastService.error(err)
      }
    });
  }

  toggleCommentBox(post: any) {
    post.showCommentBox = !post.showCommentBox;
  }

  addComment(post: any) {
    // debugger
    const req: AddCommentRequest = {
      text: post.newCommentText,
      postId: post.id,
      profileId: this.authService.getProfileId() ?? '1'
    }
    this.commentService.addComment(req).subscribe();
    if (!post.comments) {
      post.comments = [];
    }
    post.comments.push({ text: post.newCommentText })
    post.commentsCount++;
    post.newCommentText=''

  }

  likeComment(comment: CommentResponse, post: PostResponse) {
    const req: LikeCommentRequest = {
      profileId: this.profileId,
      postId: post.id,
      commentId: comment.id,
      reactionType: 1
    }

    this.commentService.likeComment(req).subscribe(() => {
      comment.isLiked = true;
      comment.reactCount++;
    });
  }

  dislikeComment(comment: CommentResponse, post: PostResponse) {
    const req: DislikeCommentRequest = {
      profileId: this.profileId,
      postId: post.id,
      commentId: comment.id,
      reactionType: 1
    }
    this.commentService.dislikeComment(req).subscribe(() => {
      comment.isLiked = false;
      comment.reactCount--;
    });
  }

  editComment(comment: any, post: any) {
    const newText = prompt("Edit comment", comment.text);
    if (!newText) return;
    let req: EditCommentRequest = {
      id: comment.id,
      text: newText,
      userId: this.userId,
      postId: post.id
    }

    this.commentService.editComment(req).subscribe(() => {
      comment.text = newText;
    });
  }

  deleteComment(post: any, comment: any) {
    this.commentService.deleteComment(comment.id).subscribe(() => {
      post.comments = post.comments.filter((c: any) => c.id !== comment.id);
      post.commentsCount--;
    });
  }
  ///**************************************post **************** */
  // Toggle menu open/close
  togglePostMenu(post: any) {
    // Close all other open menus first
    this.posts.forEach((p: any) => {
      if (p.id !== post.id) p.showMenu = false;
    });
    post.showMenu = !post.showMenu;
  }

  // Close menu when clicking anywhere outside
  @HostListener('document:click')
  onDocumentClick() {
    this.posts.forEach((p: any) => p.showMenu = false);
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (!file) return;
    this.newPostImageFile = file;

    // Show preview
    const reader = new FileReader();
    reader.onload = (e: any) => this.newPostImagePreview = e.target.result;
    reader.readAsDataURL(file);
  }

  addPost() {
    this.router.navigate(['create/post'])
  }

  editPost(post: any) {
    const req: EditPostRequest = {
      id: post.id,
      title: post.title,
      text: post.text
    }
    this.postService.editPost(post).subscribe()
  }

  deletePost(postId: string) {
    this.postService.deletePost(postId).subscribe()
  }

  hidePost(post: PostResponse) {
    this.postService.hidePost(post.id).subscribe({
      next(res) {
        post.isHidden = true;
      }
    })
  }

  searchPosts(keyword: string) {
    this.postService.searchPosts(keyword).subscribe()
  }

  toggleLike(post: any) {
    // debugger
    if (post.isLiked) {
      const disReq: DislikeRequest = {
        postId: post.id,
        profileId: this.authService.getProfileId() ?? '1'
      }
      this.likeService.dislikePost(disReq).subscribe()
      post.isLiked = false;
      post.reactsCount--;
    }
    else {
      const likeReq: LikeRequest = {
        postId: post.id,
        profileId: this.authService.getProfileId() ?? '1',
        react: 1
      }
      this.likeService.likePost(likeReq).subscribe()
      post.isLiked = true
      post.reactsCount++;
    }
  }

  loadPosts() {
    // debugger
    this.userId = this.authService.getUserId() ?? '1'
    this.profileId = this.authService.getProfileId() ?? '1'
    this.postService.getAllPosts(this.profileId).subscribe({
      next: (res: PostResponse[]) => {
        console.log(res)
        this.posts = res.map((post: any) => {
          let images = [];
          try {
            images = JSON.parse(post.mediaUrls);
          } catch {
            images = [];
          }

          return {
            ...post,
            // imageUrl: images.length ? images[0] : null,
            // comments: [],
            // showComments: false,
            // showCommentBox: false,
            // showShareBox: false
          };
        });
      },
      error: (err) => {
        this.toastService.error(err)
      }
    })
  }

  onSearch() {
    if (!this.searchQuery.trim()) {
      this.loadPosts();
      return;
    }

    this.postService.searchPosts(this.searchQuery).subscribe({
      next: (res: PostResponse[]) => {
        this.posts = res;
      },
      error: (err) => {
        this.toastService.error(err);
      }
    });
  }


  @HostListener('document:keydown.escape', ['$event'])
  handleEscape(event: KeyboardEvent) {
    if (this.isOpen) {
      this.isOpen = false;
    }
  }
  //***********************stories************************************** */
  loadStories() {
    this.storyService.getAll(this.profileId).subscribe({
      next: (res: StoryResponse[]) => {
        this.stories = res;
      },
      error: (err) => {
        this.toastService.error(err);
      }
    });
  }

  viewStory(story: StoryResponse): void {

    this.activeStoryUser = story;
    this.storyService.viewStory(this.userId, story.id).subscribe((res: StoryResponse) => {
      // res.hasUnviewedStatus=false;
    });
  }

  reactToStory(storyId: string) {
    this.storyService.addReact(this.userId, storyId).subscribe();
  }

  addStoryComment(storyId: string) {

    const req: AddStoryCommentRequest = {
      text: this.storyComment,
      profileId: this.profileId,
      StoryId: storyId
    };

    this.storyService.addStoryComment(req).subscribe({
      next: () => {
        console.log('Comment added');
        this.storyComment = '';
      }
    });
  }

  createStory() {
    this.storyService.addStory(this.newStatus, this.profileId).subscribe({
      next: () => {
        console.log('Story added');
        this.loadStories();
        this.newStatus = '';
      }
    });
  }


  deleteStory(story: any) {

    const req = {
      storyId: story.id,
      userId: this.authService.getUserId() ?? ''
    };

    this.storyService.deleteStory(req).subscribe({
      next: () => {
        this.loadStories();
        // this.usersWithStatus.pop(story)
      }
    });
  }

  loadViewers(storyId: string) {
    this.storyService.getViewers(this.userId, storyId).subscribe();
  }

}
