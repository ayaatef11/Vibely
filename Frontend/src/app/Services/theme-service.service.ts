import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeServiceService {
 
  init() {
    const saved = localStorage.getItem('theme') ?? 'light';
    this.applyTheme(saved);
  }

  applyTheme(value: string) {
    if (value === 'dark') {
      document.body.classList.add('dark-theme');
    } else if (value === 'light') {
      document.body.classList.remove('dark-theme');
    } else {
      // auto
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      document.body.classList.toggle('dark-theme', prefersDark);
    }
    localStorage.setItem('theme', value);
  }
}