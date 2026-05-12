import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageServiceService } from '../../../Services/language-service.service';

@Component({
  selector: 'app-preferences',
  standalone: true,
  imports: [FormsModule,TranslateModule],
  templateUrl: './preferences.component.html',
  styleUrl: './preferences.component.css'
})
export class PreferencesComponent {
  constructor(private languageService:LanguageServiceService){}
  ngOnInit() {
    this.selectedLanguage = localStorage.getItem('lang') || 'en';
  const savedTheme = localStorage.getItem('theme')??'light';
this.selectedTheme=savedTheme;
  if (savedTheme === 'dark') {
    this.isDark = true;
    document.body.classList.add('dark-theme');
  } 
    else if (savedTheme === 'light') {
    this.isDark = false;
    document.body.classList.remove('dark-theme');
  } 
  else {
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

    this.isDark = prefersDark;
    document.body.classList.toggle('dark-theme', prefersDark);
  }
}
//***********************VARIABLES**************** */
isDark = false;
selectedTheme: string = 'light';
selectedLanguage: string = 'en'; 
///*****************FUNCTIONS************* */
onThemeChange(event: any) {
  const value = event.target.value;

  if (value === 'dark') {
    this.isDark = true;
    document.body.classList.add('dark-theme');
    localStorage.setItem('theme', 'dark');
  } 
  else if (value === 'light') {
    this.isDark = false;
    document.body.classList.remove('dark-theme');
    localStorage.setItem('theme', 'light');
  }
  else {
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

    this.isDark = prefersDark;

    document.body.classList.toggle('dark-theme', prefersDark);

    localStorage.setItem('theme', 'auto');
  }
}
 onLanguageChange(event: any) {
    this.languageService.setLanguage(event.target.value);
  }

}