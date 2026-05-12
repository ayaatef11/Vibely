import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { ProfileResponse } from '../../Models/Profiles/Responses/ProfileResponse';
import { FollowRequest } from '../../Models/Follow/Requests/FollowRequest';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../Services/authentication-service.service';
import { FollowSerivceService } from '../Services/follow-serivce.service';

interface UserProfile {
  id: string;
  fullName: string;
  userName: string;
  bio: string;
  location: string;
  profileImage: string;
  followerCount: number;
  followingCount: number;
  postCount: number;
}
@Component({
  selector: 'app-all-people',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './all-people.component.html',
  styleUrl: './all-people.component.css'
})
export class AllPeopleComponent {
constructor(private http: HttpClient,private authService:AuthenticationService,private followService:FollowSerivceService) {}

  ngOnInit(): void {
    this.loadPeople();
  }
  //**********************VARIABLES************************** */
  users: ProfileResponse[] = [];
  followingStates: { [key: string]: 'idle' | 'loading' | 'requested' } = {};
  isLoading = true;
  error = '';
  currentUserId = this.authService.getUserId()??'1';
currentProfileId=this.authService.getProfileId()??'1';

  loadPeople(): void {
    this.isLoading = true;
    this.followService.FindPeople(this.currentProfileId).subscribe({
        next: (data) => {
          this.users = data;
          data.forEach(u => this.followingStates[u.id] = 'idle');
          this.isLoading = false;
        },
        error: () => {
          this.error = 'Failed to load users.';
          this.isLoading = false;
        }
      });
  }

  follow(receiverId: string): void {
    this.followingStates[receiverId] = 'loading';

    const req:FollowRequest = { sender: this.currentProfileId, reciever: receiverId };

    this.followService.requestFollow(req).subscribe({
      next: () => this.followingStates[receiverId] = 'requested',
      error: () => this.followingStates[receiverId] = 'idle'
    });
  }

  getInitials(fullName: string): string {
    return fullName.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2);
  }

}
