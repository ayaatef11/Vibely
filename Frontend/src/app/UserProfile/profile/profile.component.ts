import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProfileServiceService } from '../Services/profile-service.service';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';
import { ProfileResponse } from '../../../Models/Profiles/Responses/ProfileResponse';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BlocksServiceService } from '../../Common/comments/Services/blocks-service.service';
declare var bootstrap: any;

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule,RouterModule,NgIf, NgFor],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})

export class ProfileComponent {
  constructor(private router: Router,private blocksService:BlocksServiceService,private route:ActivatedRoute,private profileService: ProfileServiceService, private authService: AuthenticationService) { }
  
  ngOnInit(): void {
    this.loadProfile();
     this.checkIfCurrentUser();
  this.checkIfBlocked();
  }
  //**************************variables**************************************** */

user!: ProfileResponse;
isCurrentUser: boolean = false;
isBlocked: boolean = false;
profileId = this.route.snapshot.paramMap.get('id')??'1';
currentUserId=this.authService.getUserId()??'1'
//*************************************functions********************* */
 isWebsite(): number {
  return this.user?.website ? 1 : 0;
}

checkIfCurrentUser() {
  const currentProfileId = this.authService.getProfileId()
  this.isCurrentUser = currentProfileId === this.profileId;
}

checkIfBlocked() {
  this.blocksService.getBlockedUsers(this.currentUserId).subscribe((res: any) => {
    this.isBlocked = res.some((u: any) => u.id === this.currentUserId);
  });
}

blockUser() {
  const data = {
    blockerId: this.currentUserId,
    blockedId: this.user.socialMediaUserId
  };

  this.blocksService.blockUser(data).subscribe(() => {
    this.isBlocked = true;
  });
}

unblockUser() {
  const data = {
    blockerId: this.currentUserId,
    blockedId: this.user.socialMediaUserId
  };

  this.blocksService.unblockUser(data).subscribe(() => {
    this.isBlocked = false;
  });
}

openPost(postId: string) {
  this.router.navigate(['/user/post', postId]);
}


loadProfile() {
    debugger
    this.profileService.viewProfile(this.profileId).subscribe({
      next: (res: ProfileResponse) => {
        this.user = res;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
openModal() {
    const modal = new bootstrap.Modal(document.getElementById('profilePictureModal')!);
    modal.show();
  }

}
