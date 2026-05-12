import { Component } from '@angular/core';
import { ProfileResponse } from '../../../Models/Profiles/Responses/ProfileResponse'; 
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { EditProfileRequest } from '../../../Models/Profiles/Requests/EditProfileRequest';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { ProfileServiceService } from '../../Services/profile-service.service';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [RouterModule, FormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})
export class EditProfileComponent {
  constructor(private router: Router, private authService: AuthenticationService, private profileService: ProfileServiceService) { }
  ngOnInit() {
    this.loadProfile()
  }

  //**********************************variables***************************************** */
  user: ProfileResponse = {
    id: '',
    fullName: '',
    userName: '',
    postCount: 0,
    followerCount: 0,
    followingCount: 0,
    bio: '',
    location: '',
    website: '',
    profileImage: '',
    backgroundImage: '',
    userId: '',
    profileImageContentType: ' ',
    backgroundImageContentType: ' ',
    posts: []
  };
  userId = this.authService.getUserId() ?? ''
profileId=this.authService.getProfileId()??''
  //**********************************functions***************************** */  
  editProfile() {
    console.log(this.user)
    const req: EditProfileRequest = {
      userId: this.authService.getUserId() ?? '',
      fullName: this.user.fullName,
      userName: this.user.userName,
      bio: this.user.bio,
      location: this.user.location,
      website: this.user.website,
      profileImage: [],
      backgroundImage: []
    };

    this.profileService.editProfile(req).subscribe({
      next: (res: ProfileResponse) => {
        console.log("Profile updated");
        this.router.navigate(['/home/profile',this.profileId])

      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  loadProfile() {

    const profileId = this.authService.getProfileId() ?? '1';

    this.profileService.viewProfile(profileId).subscribe({
      next: (res: ProfileResponse) => {
        this.user = res;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
