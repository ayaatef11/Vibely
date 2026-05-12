import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule,Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AuthenticationService } from '../../Services/authentication-service.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterModule,TranslateModule,CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  constructor(private authService:AuthenticationService,private router:Router){}
ngOnInit(){
this.profileId=this.authService.getProfileId()??'1';
}
//*****************VARIABLES**************************** */
isSidebarOpen = true;
profileId!:string;
//*******************FUNCTIONS******************************* */
toggleSidebar() {
  this.isSidebarOpen = !this.isSidebarOpen;
}

logout(){
  this.authService.logout();
  this.authService.removeToken();
this.router.navigate(['/login'])
}
}
