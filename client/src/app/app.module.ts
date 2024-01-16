import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { NavComponent } from './nav/nav.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptor/error.interceptor';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtInterceptor } from './_interceptor/jwt.interceptor';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ToastrModule } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { NgxSpinner, NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './_interceptor/loading.interceptor';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DataPickerComponent } from './_forms/data-picker/data-picker.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TimeagoModule } from "ngx-timeago";




@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    TestErrorsComponent,
    HomeComponent,
    RegisterComponent,  
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DataPickerComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    NgxSpinnerModule.forRoot({
      type:'line-scale-party'
    }),
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    ButtonsModule.forRoot(),
    FileUploadModule,
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
    }),
    TimeagoModule.forRoot(),
    TabsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot()

  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},



  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
