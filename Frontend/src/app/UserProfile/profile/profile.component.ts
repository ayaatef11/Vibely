import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProfileResponse } from '../../../Models/Profiles/Responses/ProfileResponse';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatServiceService } from '../../Services/chat-service.service';
import { BlocksServiceService } from '../../Services/blocks-service.service';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { ProfileServiceService } from '../../Services/profile-service.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';
declare var bootstrap: any;

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule, RouterModule, NgIf, NgFor,TranslateModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})

export class ProfileComponent {
  constructor(private router: Router, private chatService: ChatServiceService,private toastService:ToastrService,
     private blocksService: BlocksServiceService, private route: ActivatedRoute, private profileService: ProfileServiceService, private authService: AuthenticationService) { }

  ngOnInit(): void {
    this.loadProfile();
    this.checkIfCurrentUser();
    this.checkIfBlocked();
  }
  //**************************variables**************************************** */

  profile!: ProfileResponse;
  isCurrentUser: boolean = false;
  isBlocked: boolean = false;
  profileId = this.route.snapshot.paramMap.get('id') ?? '1';
  currentUserId = this.authService.getUserId() ?? '1'
  currentProfileId = this.authService.getProfileId() ?? '1'
  //*************************************functions********************* */
  openChat(): void {
    debugger
    this.chatService.createChat(this.currentUserId, this.profile.userId).subscribe({
      next: (chat) => {
        this.router.navigate(['/home/chat/friend', chat.id]);
      },
      error: (err) => {
        this.toastService.error('Failed to create or open chat:', err);
      }
    });
  }

  isWebsite(): number {
    return this.profile?.website ? 1 : 0;
  }

  checkIfCurrentUser() {
    this.isCurrentUser = this.currentProfileId === this.profileId;
  }

  checkIfBlocked() {
    this.blocksService.getBlockedUsers(this.currentUserId).subscribe((res: any) => {
      this.isBlocked = res.some((u: any) => u.id === this.currentUserId);
    });
  }

  blockUser() {
    const data = {
      blockerId: this.currentUserId,
      blockedId: this.profile.userId
    };

    this.blocksService.blockUser(data).subscribe(() => {
      this.isBlocked = true;
    });
  }

  unblockUser() {
    const data = {
      blockerId: this.currentUserId,
      blockedId: this.profile.userId
    };

    this.blocksService.unblockUser(data).subscribe(() => {
      this.isBlocked = false;
    });
  }

  openPost(postId: string) {
    this.router.navigate(['/home/user/post', postId]);
  }

  loadProfile() {
    debugger
    this.profileService.viewProfile(this.profileId).subscribe({
      next: (res: ProfileResponse) => {
        this.profile = res;
      },
      error: (err) => {
        this.toastService.error(err);
      }
    });
  }

  openModal() {
    const modal = new bootstrap.Modal(document.getElementById('profilePictureModal')!);
    modal.show();
  }

}
