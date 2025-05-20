import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateNewOrganisationComponent } from './create-new-organisation.component';

describe('CreateNewOrganisationComponent', () => {
  let component: CreateNewOrganisationComponent;
  let fixture: ComponentFixture<CreateNewOrganisationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CreateNewOrganisationComponent]
    });
    fixture = TestBed.createComponent(CreateNewOrganisationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
