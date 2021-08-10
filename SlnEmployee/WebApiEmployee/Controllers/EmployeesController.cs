using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebApiEmployee.Controllers
{
    public class EmployeesController : ApiController
    {
        //[HttpGet]
        //public IEnumerable<Employee> LoadAllEmployees()
        //{
        //    EmployeeDBEntities entities = new EmployeeDBEntities();
        //    return entities.Employees;
        //}

        //[HttpGet]
        //public HttpResponseMessage LoadAllEmployees(string gender = "All") //http://localhost:65428/api/employees?gender=all
        //{

        //    EmployeeDBEntities entities = new EmployeeDBEntities();
        //    switch (gender.ToLower())
        //    {
        //        case "all":
        //            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees);
        //        case "male":
        //            return Request.CreateResponse(HttpStatusCode.OK,
        //               (entities.Employees.Where(e => e.Gender.ToLower() == "male" )));
        //        case "female":
        //            return Request.CreateResponse(HttpStatusCode.OK,
        //               (entities.Employees.Where(e => e.Gender.ToLower() == "female")));
        //        default:
        //            return Request.CreateResponse(HttpStatusCode.BadRequest,"Please enter a valid value for gender");

        //    }

        //}


        [HttpGet]
        [BasicAuthentication]
        public HttpResponseMessage LoadAllEmployees()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            EmployeeDBEntities entities = new EmployeeDBEntities();
            switch (username.ToLower())
            {
                 case "male":
                    return Request.CreateResponse(HttpStatusCode.OK,
                       (entities.Employees.Where(e => e.Gender.ToLower() == "male")));
                case "female":
                    return Request.CreateResponse(HttpStatusCode.OK,
                       (entities.Employees.Where(e => e.Gender.ToLower() == "female")));
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

            }

        }


        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with the id " + id + " not found.");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                EmployeeDBEntities entities = new EmployeeDBEntities();
                entities.Employees.Add(employee);
                entities.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri +"/"+ employee.ID.ToString());
                return message;
            }
            catch (Exception ex)
            {
               return  Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity != null)
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with the id " + id + " not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// if the parameter is simple type like int or bool, web api get the value form URI by default,
        /// otherwise we have to specify like [FromBody]
        /// if the parameter is complex type like employee, web api get the value form body by default,
        /// otherwise we have to specify like [FromURI]
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        ///
        public HttpResponseMessage Put(int id, [FromBody]Employee employee)//if we remove [FromBody], doesn't make any difference
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity != null)
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with the id " + id + " not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
