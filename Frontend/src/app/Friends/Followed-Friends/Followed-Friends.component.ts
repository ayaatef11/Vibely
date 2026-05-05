import { Component } from '@angular/core';
import { FriendsHeaderComponent } from "../friends-header/friends-header.component";
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';
import { FollowSerivceService } from '../Services/follow-serivce.service';
import { ProfileServiceService } from '../../UserProfile/Services/profile-service.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-followedFriends',
  standalone: true,
  imports: [FriendsHeaderComponent,NgFor,NgIf],
  templateUrl: './Followed-Friends.component.html',
  styleUrl: './Followed-Friends.component.css'
})
export class FollowedFriendsComponent {
  constructor(private authService: AuthenticationService,private profileService:ProfileServiceService){}
followedUsers!:UserResponse[]
ngOnInit(){
  this.viewFollowed()
}
viewFollowed(){
  // debugger
  const userId =this.authService.getUserId()??'1'

this.profileService.getFollowers(userId).subscribe({
  next:(res:UserResponse[])=>this.followedUsers=res,
  error:(err)=>console.error(err)
})
}

}
