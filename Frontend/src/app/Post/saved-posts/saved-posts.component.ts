import { Component } from '@angular/core'; 
import { NgFor, NgIf } from '@angular/common';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { SavedPostsServiceService } from '../../Services/saved-posts-service.service';

@Component({
  selector: 'app-saved-posts',
  standalone: true,
  imports: [NgFor,NgIf],
  templateUrl: './saved-posts.component.html',
  styleUrl: './saved-posts.component.css'
})
export class SavedPostsComponent {


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

