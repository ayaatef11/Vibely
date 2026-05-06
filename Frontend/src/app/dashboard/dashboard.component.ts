import { NgFor, NgIf, NgClass, SlicePipe } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../Common/sidebar/sidebar.component';
import { Router, RouterModule } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { SearchHeaderComponent } from "../Common/search/search-header/search-header.component";
import { CommentServiceService } from '../Common/comments/Services/comment-service.service';
import { FollowSerivceService } from '../Friends/Services/follow-serivce.service';
import { PostServiceService } from '../Post/create-post/Services/post-service.service';
import { StoryServiceService } from './Services/story-service.service';
import { AuthenticationService } from '../Auth/Services/authentication-service.service';
import { PostResponse } from '../../Models/Posts/Responses/PostResponse';
import { LikesServiceService } from '../Common/comments/Services/likes-service.service';
import { DislikeRequest } from '../../Models/Reacts/Requests/DislikeRequest';
import { LikeRequest } from '../../Models/Reacts/Requests/LikeRequest';
import { SharePostServiceService } from '../Post/create-post/Services/share-post-service.service';
import { AddCommentRequest } from '../../Models/Comments/Requests/AddCommentRequest';
import { UserServiceService } from '../UserProfile/Services/user-service.service';
import { FollowRequest } from '../../Models/Follow/Requests/FollowRequest';
import { UserResponse } from '../../Models/Users/Responses/UserResponse';
import { LikeCommentRequest } from '../../Models/Comments/Requests/LikeCommentRequest';
import { DislikeCommentRequest } from '../../Models/Comments/Requests/DislikeCommentRequest';
import { EditCommentRequest } from '../../Models/Comments/Requests/EditCommentRequest';
import { CommentResponse } from '../../Models/Posts/Responses/CommentResponse';
import { AddPostRequest } from '../../Models/Posts/Requests/AddPostRequest';
import { EditPostRequest } from '../../Models/Posts/Requests/EditPostRequest';
import { debug } from 'console';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [FormsModule, SidebarComponent, RouterModule, NgIf, NgFor, NgClass, SlicePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  animations: [
    trigger('slideInOut', [
      state('in', style({
        transform: 'translateY(0)',
        opacity: 1
      })),
      state('out', style({
        transform: 'translateY(-100%)',
        opacity: 0
      })),
      transition('in => out', animate('300ms ease-in-out')),
      transition('out => in', animate('300ms ease-in-out'))
    ])
  ]
})
export class DashboardComponent {
  constructor(private router: Router, private userService: UserServiceService, private shareService: SharePostServiceService, private likeService: LikesServiceService, private authService: AuthenticationService, private commentService: CommentServiceService, private postService: PostServiceService, private storyService: StoryServiceService, private followService: FollowSerivceService) { }


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
  usersWithStatus: any[] = [];
  suggestedUsers: UserResponse[] = [];
  friendRequests: UserResponse[] = [];
  showRequests = false;
  trendingPosts: PostResponse[] = [];
  userId!: string
  profileId!: string
  newPostText!: string
  newPostTitle!: string
  showCreatePostModal = false;
  currentUserName:string=this.authService.getUserName()??'1'
  newPostFeeling = 1;
  newPostImagePreview: string | null = null;
  newPostImageFile: File | null = null;

  //****************************** functions *************************************** */
  viewProfile(profileId: string) {
    debugger
    this.router.navigate(['/home/profile', profileId]);
  }

  goToPost(postId: string) {
    this.router.navigate(['/post', postId]);
  }
  
  loadTrendingPosts() {
    this.postService.getTrendingPosts().subscribe({
      next: (res: any) => {
        this.trendingPosts = res;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
  /////**friend requests */
  acceptRequest(req: UserResponse) {

    console.log(req)
    this.followService.acceptFollow({
      reciever: this.authService.getUserId() ?? '1',
      sender: req.id
    }).subscribe(() => {
      this.friendRequests = this.friendRequests.filter(r => r.id !== req.id);
    });
  }

  rejectRequest(req: UserResponse) {
    this.followService.rejectFollow({
      reciever: this.authService.getUserId() ?? '1',
      sender: req.id
    }).subscribe(() => {
      this.friendRequests = this.friendRequests.filter(r => r.id !== req.id);
    });
  }

  toggleRequests() {
    this.showRequests = !this.showRequests;
    this.loadFriendRequests();
  }

  loadFriendRequests() {
    const userId = this.authService.getUserId() ?? '';

    this.followService.viewRequests(userId).subscribe({
      next: (res: UserResponse[]) => {
        this.friendRequests = res;
      },
      error: (err) => {
        console.error(err);
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
    console.log("Data", data)

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
        console.error(err);
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
    alert("Link copied!");
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
        console.error(err)
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
  openCreatePost() {
    this.showCreatePostModal = true;
  }

  closeCreatePost() {
    this.showCreatePostModal = false;
    this.newPostTitle = '';
    this.newPostText = '';
    this.newPostFeeling = 1;
    this.newPostImagePreview = null;
    this.newPostImageFile = null;
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

  removeImage() {
    this.newPostImagePreview = null;
    this.newPostImageFile = null;
  }

  addPost() {
    this.router.navigate(['create/post'])
   
  }

  editPost(post: any) {
    const req: EditPostRequest = {
      id: post.id,
      feelingState: post.feelingState,
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
      next(res){
post.isHidden=true;
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
    this.postService.getAllPosts(this.userId).subscribe({
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
        console.error(err)
      }
    })
  }

  onSearch() {
    this.isOpen = !this.isOpen;
    if (this.isOpen) {
      setTimeout(() => {
        const input = document.getElementById('searchInput');
        if (input) input.focus();
      }, 300);
    }
    this.router.navigate(['/search/all'])
  }

  @HostListener('document:keydown.escape', ['$event'])
  handleEscape(event: KeyboardEvent) {
    if (this.isOpen) {
      this.isOpen = false;
    }
  }
  //***********************stories************************************** */
  loadStories() {
    const userId = this.authService.getUserId() ?? '1';

    this.followService.getFollowingWithStories(userId).subscribe({
      next: (res: any) => {

        this.usersWithStatus = res.map((user: any) => ({
          username: user.fullName,
          profileImage: user.profileImage,
          storyId: user.stories?.length ? user.stories[0].id : null,
          hasUnviewedStatus: false,
          stories: user.stories || []
        }));

      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  viewStory(user: any): void {
    const userId = '8830F83E-0549-4CA0-2707-08DEA038226A'
    const storyId = '2'
    this.storyService.viewStory(userId, storyId);
  }

}
