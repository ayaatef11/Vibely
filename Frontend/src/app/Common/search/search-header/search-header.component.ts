import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-search-header',
  standalone: true,
  imports: [RouterModule,TranslateModule],
  templateUrl: './search-header.component.html',
  styleUrl: './search-header.component.css'
})
export class SearchHeaderComponent {

}
