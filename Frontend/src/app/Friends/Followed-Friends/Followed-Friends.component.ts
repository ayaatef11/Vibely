import { Component } from '@angular/core';
import { FriendsHeaderComponent } from "../friends-header/friends-header.component";
import { UserResponse } from '../../../Models/Users/Responses/UserResponse'; 
import { NgFor, NgIf } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { ProfileServiceService } from '../../Services/profile-service.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';
import { FollowSerivceService } from '../../Services/follow-serivce.service';
import { FollowRequest } from '../../../Models/Follow/Requests/FollowRequest';
import { ProfileResponse } from '../../../Models/Profiles/Responses/ProfileResponse';
import { UnFollowRequest } from '../../../Models/Follow/Requests/UnFollowRequest';

@Component({
  selector: 'app-followedFriends',
  standalone: true,
  imports: [FriendsHeaderComponent,NgFor,NgIf,RouterModule,TranslateModule],
  templateUrl: './Followed-Friends.component.html',
  styleUrl: './Followed-Friends.component.css'
})
export class FollowedFriendsComponent {
  constructor(private router:Router,private authService: AuthenticationService,
    private profileService:ProfileServiceService,private toastService:ToastrService,private followService:FollowSerivceService){}
ngOnInit(){
  this.viewFollowed()
}
//*******************************VARIABLES********************************************************* */
followedUsers!:ProfileResponse[]
currentProfileId=this.authService.getProfileId()??'1';
//*******************************FUNCTIONS********************************************** */
viewProfile(profileId: string) {
  this.router.navigate(['/home/profile', profileId]);
}

viewFollowed(){

this.profileService.getFollowers(this.currentProfileId).subscribe({
  next:(res:ProfileResponse[])=>this.followedUsers=res,
  error:(err)=>this.toastService.error(err)
})
}

follow(user: ProfileResponse) {
  const req: FollowRequest = {
    sender: this.currentProfileId,
    reciever: user.id
  };
  this.followService.requestFollow(req).subscribe({
    next: () => {
      user.isRequested = true;   
    },
    error: (err) => this.toastService.error(err)
  });
}

unfollow(user: ProfileResponse) {
  const req: UnFollowRequest = {
    sender: this.currentProfileId,
    reciever: user.id
  };
  this.followService.unfollow(req).subscribe({
    next: () => {
      user.isFollowed = false;    
      user.isRequested = false;
    },
    error: (err) => this.toastService.error(err)
  });
}


}
