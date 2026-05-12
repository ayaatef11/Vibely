import { Component } from '@angular/core';
import { FriendsHeaderComponent } from "../friends-header/friends-header.component";
import { UserResponse } from '../../../Models/Users/Responses/UserResponse'; 
import { NgFor, NgIf } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { ProfileServiceService } from '../../Services/profile-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-followedFriends',
  standalone: true,
  imports: [FriendsHeaderComponent,NgFor,NgIf,RouterModule],
  templateUrl: './Followed-Friends.component.html',
  styleUrl: './Followed-Friends.component.css'
})
export class FollowedFriendsComponent {
  constructor(private router:Router,private authService: AuthenticationService,
    private profileService:ProfileServiceService,private toastService:ToastrService){}
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
  error:(err)=>this.toastService.error(err)
})
}

}
