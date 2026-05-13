import { Component } from '@angular/core';
import { FriendsHeaderComponent } from '../friends-header/friends-header.component';
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';
import { ProfileComponent } from '../../UserProfile/profile/profile.component';
import { NgFor, NgIf } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FollowRequest } from '../../../Models/Follow/Requests/FollowRequest';
import { UnFollowRequest } from '../../../Models/Follow/Requests/UnFollowRequest';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { FollowSerivceService } from '../../Services/follow-serivce.service';
import { ProfileServiceService } from '../../Services/profile-service.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-following-friends',
  standalone: true,
  imports: [FriendsHeaderComponent,NgFor,NgIf,RouterModule,TranslateModule],
  templateUrl: './following-friends.component.html',
  styleUrl: './following-friends.component.css'
})
export class FollowingFriendsComponent {
  constructor(private router:Router,private followService:FollowSerivceService,private toastService:ToastrService,
    private authService:AuthenticationService,private profileService:ProfileServiceService){}
  ngOnInit(){
  this.viewFollowing()
}
//*************************VARIABLES************************************** */
FollowersUsers!:UserResponse[]
currentUserId=this.authService.getUserId()??'1';
//*****************************FUNCTIONS****************************************** */
viewProfile(profileId: string) {
  this.router.navigate(['/home/profile', profileId]);
}

requestFollow(id:string){
  const req:FollowRequest={
    sender:this.currentUserId,
    reciever:id
  };
this.followService.requestFollow(req).subscribe();
}

unfollow(id:string){
const  req: UnFollowRequest={
  sender:this.currentUserId,
  reciever:id
};
this.followService.unfollow(req).subscribe();
}

viewFollowing(){
  // debugger
  const userId =this.authService.getUserId()??'1'

this.profileService.getFollowing(userId).subscribe({
  next:(res:UserResponse[])=>this.FollowersUsers=res,
  error:(err)=>this.toastService.error(err)
})
}
}
