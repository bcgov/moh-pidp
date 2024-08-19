import { Inject, Injectable } from '@angular/core';

import * as CryptoJS from 'crypto-js';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Injectable({
  providedIn: 'root',
})
export class CryptoService {
  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {}

  public encrypt(plainText: string): string {
    return CryptoJS.AES.encrypt(
      plainText,
      this.config.cookies.secret_key,
    ).toString();
  }

  public decrypt(cipherText: string): string {
    return CryptoJS.AES.decrypt(
      cipherText,
      this.config.cookies.secret_key,
    ).toString(CryptoJS.enc.Utf8);
  }
}
