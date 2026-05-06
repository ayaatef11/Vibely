import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-preferences',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './preferences.component.html',
  styleUrl: './preferences.component.css'
})
export class PreferencesComponent {
  constructor(){}
  ngOnInit() {
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
}