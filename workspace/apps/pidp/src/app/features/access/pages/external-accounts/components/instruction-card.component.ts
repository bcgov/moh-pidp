import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  computed,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';

import { InstructionCard } from './instruction-card.model';

@Component({
  selector: 'app-instruction-card',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
  ],
  templateUrl: './instruction-card.component.html',
  styleUrl: './instruction-card.component.scss',
})
export class InstructionCardComponent {
  @Input()
  public cardData: InstructionCard = {} as InstructionCard;
  @Input() public routePath = '';

  @Input()
  public isActive = false;

  @Input()
  public isCompleted = false;

  @Output()
  public continueEvent = new EventEmitter<string>();

  public email = this.cardData.placeholder;

  public searchControl = new FormControl('');

  public searchTerm = signal('');
  public showResults = signal(false);
  public isLoading = signal(false);
  public selectedDomain = signal('');

  private readonly domains = [
    'example.com',
    'example.org',
    'extension.net',
    'extenstion.edu',
    'Yukon.gov',
    'Yuking.ca',
  ];

  // Convert the observable to a signal
  private searchResultsData = toSignal(
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term) => {
        if (!term || term.length < 2) {
          this.isLoading.set(false);
          return of([]);
        }

        this.isLoading.set(true);
        this.searchTerm.set(term);
        return of(
          this.domains.filter((domain) =>
            domain.toLowerCase().startsWith(term.toLowerCase()),
          ),
        );
      }),
    ),
    { initialValue: [] },
  );

  // Computed signal for filtered results
  searchResults = computed(() => {
    return this.searchResultsData() || [];
  });

  public onContinue(value?: string): void {
    console.log('onContinue', value);

    this.selectedDomain.set(value || '');
    const localPart = this.searchTerm();
    const domain = this.selectedDomain();
    if (localPart && domain) {
      this.email = `${localPart}@${domain}`;
    }
    if (this.cardData.type === 'input') {
      this.continueEvent.emit(this.email);
    } else {
      this.continueEvent.emit(value);
    }
  }
}
