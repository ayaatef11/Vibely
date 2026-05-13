import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageServiceService } from './Services/language-service.service';
import { ThemeServiceService } from './Services/theme-service.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,TranslateModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
 constructor(private languageService: LanguageServiceService,private themeService:ThemeServiceService) {}

  ngOnInit() {
    // debugger
    this.languageService.init(); 
     this.themeService.init();
  }
  title = 'Vibely';

}
