import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DislikeRequest } from '../../../Models/Reacts/Requests/DislikeRequest';
import { Post } from '../../../DTOS/Post';
import { PostResponse } from '../../../Models/Posts/Responses/PostResponse';
import { LikeRequest } from '../../../Models/Reacts/Requests/LikeRequest';
import { AddCommentRequest } from '../../../Models/Comments/Requests/AddCommentRequest';
import { FormsModule } from '@angular/forms';
import { SavePostRequest } from '../../../Models/Posts/Requests/SavePostRequest';
import { UnSavePostRequest } from '../../../Models/Posts/Requests/UnSavePostRequest';
import { SidebarComponent } from '../../Common/sidebar/sidebar.component';
import { EditPostRequest } from '../../../Models/Posts/Requests/EditPostRequest';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { CommentServiceService } from '../../Services/comment-service.service';
import { LikesServiceService } from '../../Services/likes-service.service';
import { PostServiceService } from '../../Services/post-service.service';
import { SavedPostsServiceService } from '../../Services/saved-posts-service.service';
import { SharePostServiceService } from '../../Services/share-post-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-post',
  standalone: true,
  imports: [RouterModule, FormsModule, SidebarComponent, NgFor, NgIf,CommonModule],
  templateUrl: './user-post.component.html',
  styleUrl: './user-post.component.css'
})
export class UserPostComponent {

  constructor(private router: Router, private route: ActivatedRoute, private toastService: ToastrService,
    private savedPostService: SavedPostsServiceService, private likeService: LikesServiceService, private authService: AuthenticationService, private commentService: CommentServiceService, private shareService: SharePostServiceService, private postService: PostServiceService) { }
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
    text: ''
  };



  //************************FUNCTIONS********************************************** */

  loadPost(postId: string) {
    this.postService.getPost(postId,this.profileId).subscribe(res => {
      this.post = res;
    });
  }

  openEdit() {
    this.isEditOpen = true;
    this.post.showMenu = false;
    this.editData = {
      id: this.post.id,
      title: this.post.title,
      text: this.post.text
    };

  }

  saveEdit() {
    this.postService.editPost(this.editData).subscribe((res: PostResponse) => {
      this.post = res;
    });
  }

  closeEdit() {
    this.isEditOpen = false;
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