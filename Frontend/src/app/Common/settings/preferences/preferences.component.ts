import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageServiceService } from '../../../Services/language-service.service';
import { ThemeServiceService } from '../../../Services/theme-service.service';

@Component({
  selector: 'app-preferences',
  standalone: true,
  imports: [FormsModule,TranslateModule],
  templateUrl: './preferences.component.html',
  styleUrl: './preferences.component.css'
})
export class PreferencesComponent {
  constructor(private languageService:LanguageServiceService,private themeService:ThemeServiceService){}
  ngOnInit() {
    this.selectedLanguage = localStorage.getItem('lang') || 'en';
  this.selectedTheme = localStorage.getItem('theme')??'light';  
}
//***********************VARIABLES**************** */
isDark = false;
selectedTheme: string = 'light';
selectedLanguage: string = 'en'; 
///*****************FUNCTIONS************* */
onThemeChange(event: any) {
  this.selectedTheme = event.target.value;
  this.themeService.applyTheme(event.target.value); // delegates all logic
}
 onLanguageChange(event: any) {
    this.languageService.setLanguage(event.target.value);
  }

}