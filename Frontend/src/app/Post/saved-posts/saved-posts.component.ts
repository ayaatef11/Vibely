import { Component } from '@angular/core'; 
import { NgFor, NgIf } from '@angular/common';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { SavedPostsServiceService } from '../../Services/saved-posts-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-saved-posts',
  standalone: true,
  imports: [NgFor,NgIf],
  templateUrl: './saved-posts.component.html',
  styleUrl: './saved-posts.component.css'
})
export class SavedPostsComponent {

 constructor(private savedPostService: SavedPostsServiceService,private authService:AuthenticationService,private toastService:ToastrService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId()??'1';
    this.getSavedPosts();
  }
  //*******************************variables********************************** */
 savedPosts: any[] = [];
  userId: string = '';

 
//*******************************functions******************************************** */
  getSavedPosts() {
    this.savedPostService.getSavedPosts(this.userId).subscribe({
      next: (res: any) => {
        this.savedPosts = res;
        this.toastService.success('Saved posts:', res);
      },
      error: (err) => {
        this.toastService.error('Error fetching saved posts', err);
      }
    });
  }
 
}

