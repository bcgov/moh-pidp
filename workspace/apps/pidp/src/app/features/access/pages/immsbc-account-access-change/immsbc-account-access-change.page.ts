import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '@app/features/access/access.routes';

@Component({
  selector: 'app-immsbc-account-access-change',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
  ],
  templateUrl: './immsbc-account-access-change.page.html',
  styleUrl: './immsbc-account-access-change.page.scss',
})
export class ImmsbcAccountAccessChangePage implements OnInit {
  public breadcrumbsData: Array<{ title: string; path: string }> = [];
  public form!: FormGroup;

  public readonly accessLevels = [
    {
      value: 'clinician',
      title: 'Clinician',
      description: 'Clerk plus administer vaccinations and view appointments.',
    },
    {
      value: 'clerk',
      title: 'Clerk',
      description: 'Basic access to check-in patients.',
    },
    {
      value: 'remove',
      title: 'Remove Access',
      description: 'No longer employed with our pharmacy, please remove access.',
    },
  ];

  private readonly fb = inject(FormBuilder);

  public ngOnInit(): void {
    this.breadcrumbsData = [
      { title: 'Home', path: '' },
      {
        title: 'Access',
        path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
      },
      {
        title: 'ImmsBC',
        path: AccessRoutes.routePath(AccessRoutes.IMMSBC),
      },
      { title: 'Account Access Change', path: '' },
    ];

    this.form = this.fb.group({
      pharmacyManagerName: ['', Validators.required],
      pharmacyManagerEmail: ['', [Validators.required, Validators.email]],
      pharmacyName: ['', Validators.required],
      pharmacareCode: ['', Validators.required],
      bcProviderUsername: ['', Validators.required],
      collegeId: [''],
      mobilePhone: ['', Validators.required],
      accessLevel: ['', Validators.required],
    });
  }

  public selectAccessLevel(level: string): void {
    this.form.patchValue({ accessLevel: level });
  }

  public onSubmit(): void {
    if (this.form.valid) {
      console.log(this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }
}
