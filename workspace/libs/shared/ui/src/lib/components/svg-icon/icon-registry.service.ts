// import { Injectable, inject } from '@angular/core';
// import { MatIconRegistry } from '@angular/material/icon';
// import { DomSanitizer } from '@angular/platform-browser';

// @Injectable({
//   providedIn: 'root',
// })
// export class IconRegistryService {
//   private readonly iconPath = 'assets/icons/';

//   private matIconRegistry = inject(MatIconRegistry);
//   private sanitizer = inject(DomSanitizer);

//   public registerIcon(iconName: string, fileName: string): void {
//     this.matIconRegistry.addSvgIcon(
//       iconName,
//       this.sanitizer.bypassSecurityTrustResourceUrl(
//         `${this.iconPath}${fileName}.svg`,
//       ),
//     );
//   }

//   public registerIcons(icons: { name: string; file: string }[]): void {
//     icons.forEach((icon) => {
//       this.registerIcon(icon.name, icon.file);
//     });
//   }

//   public preloadIcons(): void {
//     const comminIcons = [
//       {
//         name: 'instruction-document',
//         file: './assets/images/icons/instruction-document.svg',
//       },
//       { name: 'instruction-pencil', file: 'instruction-pencil' },
//       { name: 'instruction-mail', file: 'instruction-mail' },
//       { name: 'icon-complete', file: 'icon-complete' },
//     ];

//     this.registerIcons(comminIcons);
//   }
// }
