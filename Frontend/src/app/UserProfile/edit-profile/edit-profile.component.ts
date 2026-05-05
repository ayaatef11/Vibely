import { Component } from '@angular/core';
import { ProfileResponse } from '../../../Models/Profiles/Responses/ProfileResponse';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';
import { ProfileServiceService } from '../Services/profile-service.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})
export class EditProfileComponent {
  constructor(private authService:AuthenticationService,private profileService:ProfileServiceService){}
user!:ProfileResponse;
ngOnInit(){
this.loadProfile()
}
editProfile() {
   const req = {
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
    next: (res) => {
      console.log("Profile updated");
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
