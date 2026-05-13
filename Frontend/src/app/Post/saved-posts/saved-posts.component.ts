import { Component } from '@angular/core'; 
import { NgFor, NgIf } from '@angular/common';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { SavedPostsServiceService } from '../../Services/saved-posts-service.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ProfileServiceService } from '../../Services/profile-service.service';
import { PostResponse } from '../../../Models/Posts/Responses/PostResponse';
 import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-saved-posts',
  standalone: true,
  imports: [NgFor,NgIf],
  templateUrl: './saved-posts.component.html',
  styleUrl: './saved-posts.component.css'
})
export class SavedPostsComponent {

 constructor(private router:Router,private profileService:ProfileServiceService,private savedPostService: SavedPostsServiceService,private authService:AuthenticationService,private toastService:ToastrService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId()??'1';
    this.getSavedPosts();
  }
  //*******************************variables********************************** */
 savedPosts: PostResponse[] = [];
  userId: string = '';

 
//*******************************functions******************************************** */

getSavedPosts() {
  this.savedPostService.getSavedPosts(this.userId).subscribe({
    next: (res: any) => {
      this.savedPosts = res;
      const requests = this.savedPosts.map(post =>
        this.profileService.viewProfile(post.profileId)
      );
      forkJoin(requests).subscribe(profiles => {
        profiles.forEach((profile, i) => {
          this.savedPosts[i].userName = profile.userName;
        });
      });
    },
    error: (err) => {
      this.toastService.error('Error fetching saved posts', err);
    }
  });
}

  goToPost(postId: string) {
    this.router.navigate(['/home/user/post', postId]);
  }
 
}

