import { Component } from '@angular/core';
import { FriendsHeaderComponent } from "../friends-header/friends-header.component";
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';
import { FollowSerivceService } from '../Services/follow-serivce.service';
import { ProfileServiceService } from '../../UserProfile/Services/profile-service.service';
import { NgFor, NgIf } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-followedFriends',
  standalone: true,
  imports: [FriendsHeaderComponent,NgFor,NgIf,RouterModule],
  templateUrl: './Followed-Friends.component.html',
  styleUrl: './Followed-Friends.component.css'
})
export class FollowedFriendsComponent {
  constructor(private router:Router,private authService: AuthenticationService,private profileService:ProfileServiceService){}
ngOnInit(){
  this.viewFollowed()
}
//*******************************VARIABLES********************************************************* */
followedUsers!:UserResponse[]

//*******************************FUNCTIONS********************************************** */
viewProfile(profileId: string) {
  this.router.navigate(['/home/profile', profileId]);
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
