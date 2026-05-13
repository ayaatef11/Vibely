import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-friends-header',
  standalone: true,
  imports: [RouterModule,TranslateModule],
  templateUrl: './friends-header.component.html',
  styleUrl: './friends-header.component.css'
})
export class FriendsHeaderComponent {

}
