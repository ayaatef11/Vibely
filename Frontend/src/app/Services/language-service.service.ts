import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LanguageServiceService {

  constructor(private translate: TranslateService) {}
   init() {
    const saved = localStorage.getItem('lang') || 'en';
    this.setLanguage(saved);
  }

  setLanguage(lang: string) {
    this.translate.use(lang);
    localStorage.setItem('lang', lang);

    // document.documentElement.dir = lang === 'ar' ? 'rtl' : 'ltr';
    document.documentElement.lang = lang;
  }

  getCurrentLang(): string {
    return this.translate.currentLang;
  }
}
