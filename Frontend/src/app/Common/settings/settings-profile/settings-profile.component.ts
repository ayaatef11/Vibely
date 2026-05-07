import { Component } from '@angular/core';
import { AuthenticationService } from '../../../Auth/Services/authentication-service.service';
import { ProfileResponse } from '../../../../Models/Profiles/Responses/ProfileResponse';
import { ProfileServiceService } from '../../../UserProfile/Services/profile-service.service';
import { FormsModule } from '@angular/forms';
import { EditProfileRequest } from '../../../../Models/Profiles/Requests/EditProfileRequest';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-settings-profile',
  standalone: true,
  imports: [FormsModule,TranslateModule],
  templateUrl: './settings-profile.component.html',
  styleUrl: './settings-profile.component.css'
})
export class SettingsProfileComponent {
constructor(private authService:AuthenticationService,private profileService:ProfileServiceService){}
ngOnInit(){
  this.viewProfile();
}
currentUser!:ProfileResponse;

profileId=this.authService.getProfileId()??'1'
viewProfile(){
this.profileService.viewProfile(this.profileId).subscribe((res:ProfileResponse)=>{
   this.currentUser = {
      ...res,
      bio: res.bio.length!=0 ?res.bio: '',
      location: res.location ?? null
    };
})
}
editProfile() {
    const req: EditProfileRequest = {
      userId: this.authService.getUserId() ?? '',
      fullName: this.currentUser.fullName,
      userName: this.currentUser.userName,
      bio: this.currentUser.bio,
      location: this.currentUser.location,
      website: this.currentUser.website,
      profileImage: [],
      backgroundImage: []
    };

    this.profileService.editProfile(req).subscribe({
      next: (res: ProfileResponse) => {
        console.log("Profile updated");
this.currentUser=res;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
