import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganisationPageComponent } from './organisation-page.component';

describe('OrganisationPageComponent', () => {
  let component: OrganisationPageComponent;
  let fixture: ComponentFixture<OrganisationPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrganisationPageComponent]
    });
    fixture = TestBed.createComponent(OrganisationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
