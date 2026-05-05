import { Component } from '@angular/core';
import { FriendsHeaderComponent } from '../friends-header/friends-header.component';
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';
import { ProfileComponent } from '../../UserProfile/profile/profile.component';
import { ProfileServiceService } from '../../UserProfile/Services/profile-service.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-following-friends',
  standalone: true,
  imports: [FriendsHeaderComponent,NgFor,NgIf],
  templateUrl: './following-friends.component.html',
  styleUrl: './following-friends.component.css'
})
export class FollowingFriendsComponent {
  constructor(private authService:AuthenticationService,private profileService:ProfileServiceService){}
FollowersUsers!:UserResponse[]
ngOnInit(){
  this.viewFollowing()
}
viewFollowing(){
  // debugger
  const userId =this.authService.getUserId()??'1'

this.profileService.getFollowing(userId).subscribe({
  next:(res:UserResponse[])=>this.FollowersUsers=res,
  error:(err)=>console.error(err)
})
}
}
