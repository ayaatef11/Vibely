import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SavedPostsServiceService } from '../Services/saved-posts-service.service';
import { AuthenticationService } from '../../../Auth/Services/authentication-service.service';

@Component({
  selector: 'app-saved-posts',
  standalone: true,
  imports: [RouterModule,NgFor,NgIf],
  templateUrl: './All.component.html',
  styleUrl: './All.component.css'
})
export class AllComponent {
 
 savedPosts: any[] = [];
  userId: string = '';

  constructor(private savedPostService: SavedPostsServiceService,private authService:AuthenticationService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId()??'1';
    this.getSavedPosts();
  }

  getSavedPosts() {
    this.savedPostService.getSavedPosts(this.userId).subscribe({
      next: (res: any) => {
        this.savedPosts = res;
        console.log('Saved posts:', res);
      },
      error: (err) => {
        console.error('Error fetching saved posts', err);
      }
    });
  }
 
}
