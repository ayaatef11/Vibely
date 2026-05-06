import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PostServiceService } from '../create-post/Services/post-service.service';
import { DislikeRequest } from '../../../Models/Reacts/Requests/DislikeRequest';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';
import { CommentServiceService } from '../../Common/comments/Services/comment-service.service';
import { SharePostServiceService } from '../create-post/Services/share-post-service.service';
import { Post } from '../../../DTOS/Post';
import { PostResponse } from '../../../Models/Posts/Responses/PostResponse';
import { LikesServiceService } from '../../Common/comments/Services/likes-service.service';
import { LikeRequest } from '../../../Models/Reacts/Requests/LikeRequest';
import { AddCommentRequest } from '../../../Models/Comments/Requests/AddCommentRequest';
import { FormsModule } from '@angular/forms';
import { SavedPostsServiceService } from '../SavedPosts/Services/saved-posts-service.service';
import { SavePostRequest } from '../../../Models/Posts/Requests/SavePostRequest';
import { UnSavePostRequest } from '../../../Models/Posts/Requests/UnSavePostRequest';
import { SidebarComponent } from '../../Common/sidebar/sidebar.component';
import { EditPostRequest } from '../../../Models/Posts/Requests/EditPostRequest';

@Component({
  selector: 'app-user-post',
  standalone: true,
  imports: [RouterModule, FormsModule, SidebarComponent, NgFor, NgIf],
  templateUrl: './user-post.component.html',
  styleUrl: './user-post.component.css'
})
export class UserPostComponent {

  constructor(private router: Router, private route: ActivatedRoute, private savedPostService: SavedPostsServiceService, private likeService: LikesServiceService, private authService: AuthenticationService, private commentService: CommentServiceService, private shareService: SharePostServiceService, private postService: PostServiceService) { }
ngOnInit() {
    const postId = this.route.snapshot.paramMap.get('id');

    if (postId) {
      this.loadPost(postId);
    }
  }
  //********************************************VARIABLES******************************************* */
  post!: PostResponse;
  profileId = this.authService.getProfileId() ?? '1'
  isEditOpen = false;

editData: EditPostRequest = {
  id: '',
  title: '',
  text: '',
  feelingState: 0
};


  
//************************FUNCTIONS********************************************** */

  loadPost(postId: string) {
    this.postService.getPost(postId).subscribe(res => {
      this.post = res;
    });
  }
  editPost(post: any) {
    this.editData = {
      id: post.id,
      feelingState: post.feelingState,
      title: post.title,
      text: post.text
    };
    this.isEditOpen = true;
    this.postService.editPost(post).subscribe()
  }

  deletePost() {
     
    this.postService.deletePost(this.post.id).subscribe(() => {
      this.router.navigate(['/home/profile', this.profileId])
    })
  }

  togglePostMenu() {
    this.post.showMenu = !this.post.showMenu;
  }

  togglePost(postId: string) {
    const userId = this.authService.getUserId() ?? '1'
    this.post.isSaved = !this.post.isSaved;

    if (!this.post.isSaved) {
      let req: UnSavePostRequest = {
        userId: userId,
        postId: postId
      }
      this.savedPostService.unSavePost(req).subscribe({
        next: (res: any) => {
          if (res.message == "Successfully") {
            this.post.isSaved = false;
          }
        }
      })
    } else {
      let req: SavePostRequest = {
        userId: userId,
        postId: postId
      }
      this.savedPostService.savePost(req).subscribe({
        next: (res: any) => {
          if (res.message == "Post saved successfully") {
            this.post.isSaved = true;
          }
        }
      })
    }
  }

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

  loadComments(post: any) {
    // debugger
    if (post.showComments) {
      post.showComments = false
      return;
    }
    this.commentService.getComment(post.id).subscribe({
      next: (res: any) => {
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

}