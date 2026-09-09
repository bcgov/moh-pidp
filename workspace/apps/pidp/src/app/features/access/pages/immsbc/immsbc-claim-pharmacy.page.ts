import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

import { BehaviorSubject, Subject, debounceTime, distinctUntilChanged, filter, switchMap, takeUntil, catchError, of, tap } from 'rxjs';

import { ToastService } from '@app/core/services/toast.service';
import { PharmacyResource } from './pharmacy-resource.service';
import { Pharmacy } from './pharmacy-staff.model';
import { AsyncPipe } from '@angular/common';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '@app/features/access/access.routes';

@Component({
  selector: 'app-immsbc-claim-pharmacy',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    AsyncPipe,
    BreadcrumbComponent,
    InjectViewportCssClassDirective,
  ],
  templateUrl: './immsbc-claim-pharmacy.page.html',
  styleUrl: './immsbc-claim-pharmacy.page.scss',
})
export class ImmsbcClaimPharmacyPage implements OnInit, OnDestroy {
  private readonly fb = inject(FormBuilder);
  private readonly pharmacyResource = inject(PharmacyResource);
  private readonly toastService = inject(ToastService);
  private readonly router = inject(Router);
  
  public readonly searchControl = this.fb.control('');
  
  public readonly pharmacies$ = new BehaviorSubject<Pharmacy[]>([]);
  public readonly errorMsg$ = new BehaviorSubject<string>('');
  public readonly isLoading$ = new BehaviorSubject<boolean>(false);
  public breadcrumbsData: Array<{ title: string; path: string }> = [];
  
  private readonly destroy$ = new Subject<void>();

  public ngOnInit(): void {
    this.breadcrumbsData = [
      { title: 'Home', path: '' },
      { title: 'Access', path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS) },
      { title: 'ImmsBC', path: AccessRoutes.routePath(AccessRoutes.IMMSBC) },
      { title: 'Claim your Pharmacy in ImmsBC', path: '' },
    ];

    this.searchControl.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(300),
        distinctUntilChanged(),
        tap(() => {
          this.errorMsg$.next('');
          this.pharmacies$.next([]);
        }),
        filter((query) => !!query && query.length >= 3),
        tap(() => this.isLoading$.next(true)),
        switchMap((query) => 
          this.pharmacyResource.searchPharmacies(query as string).pipe(
            catchError((err: HttpErrorResponse) => {
              if (err.status === 400 && err.error) {
                this.errorMsg$.next(err.error?.detail || err.error || 'Too many results to list. Please refine your search.');
              } else {
                this.errorMsg$.next('An error occurred while searching. Please try again.');
              }
              return of([]);
            })
          )
        ),
        tap(() => this.isLoading$.next(false))
      )
      .subscribe((results) => {
        if (!this.errorMsg$.value) {
          this.pharmacies$.next(results);
          if (results.length === 0) {
             this.errorMsg$.next('No pharmacies found matching your search.');
          }
        }
      });
  }

  public onClaim(pharmacyId: number): void {
    this.pharmacyResource.claimPharmacy(pharmacyId).subscribe({
      next: () => {
        this.toastService.openSuccessToast('Pharmacy claimed successfully.');
        this.router.navigate(['/access/immsbc/manage-pharmacy']);
      },
      error: () => {
        this.toastService.openErrorToast('An error occurred while claiming the pharmacy.');
      }
    });
  }

  public goBack(): void {
    this.router.navigate(['/access/immsbc']);
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
