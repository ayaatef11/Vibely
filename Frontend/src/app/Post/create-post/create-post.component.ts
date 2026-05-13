import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';
import { ProfileResponse } from '../../../Models/Profiles/Responses/ProfileResponse';
import { profile } from 'console';
import { AddPostRequest } from '../../../Models/Posts/Requests/AddPostRequest';
import { PostResponse } from '../../../Models/Posts/Responses/PostResponse';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { PostServiceService } from '../../Services/post-service.service';
import { ProfileServiceService } from '../../Services/profile-service.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [RouterModule, NgIf, FormsModule,TranslateModule,NgFor],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.css'
})
export class CreatePostComponent {
  constructor(private router: Router, private authService: AuthenticationService, private toastService: ToastrService,
    private profileService: ProfileServiceService, private postService: PostServiceService) { }
  ngOnInit() {
    // debugger
    this.loadUSer();
  }
  //*********************************variables****************************************************** */
  newStatus = '';
  post: AddPostRequest = {
    feelingState: 1,
    title: '',
    text: '',
    ProfileId: this.authService.getProfileId() ?? '1',
    media: []
  };
  profileId: string = this.authService.getProfileId() ?? '1';
newPostImagePreviews: string[] = [];
  currentUser!: ProfileResponse;
  //****************************************functions**************************************************** */

  loadUSer() {
    this.profileService.viewProfile(this.profileId).subscribe({
      next: (res: ProfileResponse) => {
        this.currentUser = res;
      },
      error: (err: any) => {
        this.toastService.error(err)
      }
    })
  }

  addPost() {
    debugger
    this.postService.addPost(this.post).subscribe((res: PostResponse) => {
      this.closeModal();
      this.router.navigate(['/home'])

    });
  }

 onImageSelected(event: any) {
  const files: FileList = event.target.files;
  if (!files || files.length === 0) return;

  this.post.media = this.post.media ?? [];

  Array.from(files).forEach(file => {
    this.post.media!.push(file);

    const reader = new FileReader();
    reader.onload = (e: any) => this.newPostImagePreviews.push(e.target.result);
    reader.readAsDataURL(file);
  });
}

  removeImage(index: number) {
  this.newPostImagePreviews.splice(index, 1);
  this.post.media?.splice(index, 1);
}

  closeModal() {
    // Reset all fields
    this.post = {
      feelingState: 1,
      title: '',
      text: '',
      ProfileId: this.authService.getProfileId() ?? '1',
      media: []
    };
  this.newPostImagePreviews = [];
  }
}
