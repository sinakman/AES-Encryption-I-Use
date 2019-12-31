using DAL.ORM.Entity;
using DAL.ORM.Map;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ORM.Context
{
    //Go to SaveChanges() Method...

    public class ProjectContext:DbContext
    {
        
        //Some places have been cut to draw attention to the algorithm!!!

        //is Here.
        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added).ToList();

            string identity = WindowsIdentity.GetCurrent().Name;
            string computerName = Environment.MachineName;
            DateTime dateTime = DateTime.Now;

            foreach (var item in modifiedEntries)
            {
                BaseEntity baseEntity = item.Entity as BaseEntity;

                if (item!=null)
                {
                    if (item.State==EntityState.Added)
                    {
                        baseEntity.CreatedUserName = identity;
                        baseEntity.CreatedCompName = computerName;
                        baseEntity.CreateDate = dateTime;
                    }

                    else if (item.State==EntityState.Modified)
                    {
                        baseEntity.UpdatedUserName = identity;
                        baseEntity.UpdateCompnName = computerName;
                        baseEntity.UpdateDate = dateTime;
                    }
                }
            }
            foreach (var item in modifiedEntries)
            {
                Employee employee = item.Entity as Employee;
                if (item!=null)
                {
                    if (item.State==EntityState.Added)
                    {
                        //Call Crypto Method when entity added.
                        employee.Password = PassCrypto.base64Encode(employee.Password);
                    }
                    else if (item.State == EntityState.Modified)
                    {
                        //Call Crypto Method when entity modified.
                        employee.Password = PassCrypto.base64Encode(employee.Password);
                    }

                }
            }

            return base.SaveChanges();
        }

    }
}
