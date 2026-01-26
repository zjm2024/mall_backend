using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
using System.Data;


namespace shopadminService.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public ProductService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }

        public List<Products> getProductsPageList(int pageIndex, int pageSize, int appType, int businessId, string? productName, int? productStatus, string? categoryIds, out int totalCount)
        {
            //加排序

            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "ProductId", OrderByType = OrderByType.Desc });

            /*

            Expression expbody = null;

            ParameterExpression param = Expression.Parameter(typeof(Products), "it");
            Expression expappType = Expression.Equal(Expression.Property(param, "AppType"), Expression.Constant(appType));

            expbody = expappType;


            if (productStatus != null)
            {
                Expression expstatus = Expression.Equal(Expression.Property(param, "ProductStatus"), Expression.Constant(productStatus));
                expbody = Expression.AndAlso(expappType, expstatus);
            }


            Expression<Func<Products, bool>> lambdaExpression = Expression.Lambda<Func<Products, bool>>(
                expbody,
                param
            );
            */

            //加查询条件
            var conModels = new List<IConditionalModel>();
            conModels.add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });
            conModels.add(new ConditionalModel { FieldName = "BusinessId", ConditionalType = ConditionalType.Equal, FieldValue = businessId.toString() });

            if (productName != null)
            {
                conModels.add(new ConditionalModel { FieldName = "ProductName", ConditionalType = ConditionalType.Like, FieldValue = productName.toString() });
            }

            if (productStatus != null)
            {
                conModels.add(new ConditionalModel { FieldName = "ProductStatus", ConditionalType = ConditionalType.Equal, FieldValue = productStatus.toString() });
            }
            if (categoryIds != null)
            {
                string[] categoryids = categoryIds.Split(',');
                var con = new ConditionalCollections();

                var conditionalLists = new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>();

                for (int i = 0; i < categoryids.Length; i++)
                {
                    string categoryid = categoryids[i];


                    if (i == 0)
                    {
                        var kk = new KeyValuePair<WhereType, ConditionalModel>(
                             WhereType.And,
                             new ConditionalModel() { FieldName = "TreePath", ConditionalType = ConditionalType.LikeLeft, FieldValue = categoryid.toString() });

                        conditionalLists.Add(kk);

                    }


                    else
                    {

                        var bb = new KeyValuePair<WhereType, ConditionalModel>(
                          WhereType.Or,
                          new ConditionalModel() { FieldName = "CategoryId", ConditionalType = ConditionalType.LikeLeft, FieldValue = categoryid.toString() });

                        conditionalLists.Add(bb);


                    }


                }
                con.ConditionalList = conditionalLists;
                conModels.add(con);

                //onModels.add(new ConditionalModel { FieldName = "CategoryId", ConditionalType = ConditionalType.Like, FieldValue = categoryIds.toString() });

            }
            var list = GetPageList<Products>(pageIndex, pageSize, out totalCount, conModels, orderbyList);
            return list;
        }
        


        /// <summary>
        /// 添加或更新商品
        /// </summary>
        /// <param name="Categories">分类cVO</param>

        public ResultObject updateProducts(Products pV0,string[] updateColums = null,string delSpecsids="")
        {
            //判断商品名称是否重复
            string productName = pV0.ProductName;
            int productId = pV0.ProductId;
            if (productId == 0)
            {
                var isExist = RecordExist<Products, dynamic>(it => it.ProductName == productName, it => it.ProductId);

                if (isExist)
                {
                    return new ResultObject() { Flag = 0, Message = "商品名称已存在!", Result = null };
                }
            }
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                if (productId == 0)
                {

                    int id = Add<Products>(pV0);

                    //如果有传递删除规格Ids 则先删除规格
                    bool delSuccess = true;
                    if (delSpecsids!="")
                    {
                        List<int> listids = delSpecsids.Split(',').Select(int.Parse).ToList();
                        delSuccess = Delete<ProductSpecs>(listids);
                  
                    }
                    if (!delSuccess)
                    {
                        throw new Exception();
                    }


                    //保存规格
                    if (id > 0)
                    {
                        foreach (ProductSpecs item in pV0.ProductSpecs)
                        {
                            item.AppType = pV0.AppType;
                            item.BusinessId = pV0.BusinessId;
                            item.ProductId = id;
             
                            if (item.SpecId == 0)
                            {
                                var detailid = Add<ProductSpecs>(item);
                                if (detailid == 0)
                                    throw new Exception();
                            }

                            else
                            {
                                var success = Update<ProductSpecs>(item);
                                if (!success)
                                    throw new Exception();
                            }
                        }
                    }
                  
               
                    if (id > 0)
                        resultobj= new ResultObject() { Flag = 1, Message = "添加成功!", Result = id };
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

                }
                else
                {
                    pV0.UpdateTime = DateTime.Now;

                    Array.Resize(ref updateColums, updateColums.Length + 1);
                    updateColums[updateColums.Length - 1] = "updateTime";
            
                    bool isSuccess = Update<Products>(pV0, updateColums);
                    if (isSuccess)
                    {
                        //保存规格

                        foreach (ProductSpecs item in pV0.ProductSpecs)
                        {
                            item.AppType = pV0.AppType;
                            item.BusinessId = pV0.BusinessId;
                            item.ProductId = pV0.ProductId;
                            if (item.SpecId == 0)
                            {
                                var detailid = Add<ProductSpecs>(item);
                                if (detailid == 0)
                                    throw new Exception();
                            }

                            else
                            {
                                item.UpdateTime = DateTime.Now;
                                var success = Update<ProductSpecs>(item);
                                if (!success)
                                    throw new Exception();
                            }
                        }

                    }

                        if (isSuccess)
                        resultobj= new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            
                _db.Ado.CommitTran();

                return resultobj;
            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }


        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id">商品id</param>
        public ResultObject deleteProducts(int id)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;

                bool isSuccess = Delete<Products>(id);
                if (isSuccess)
                {
                    resultobj= new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
                }
                else
                {
                    resultobj= new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }

                _db.Ado.CommitTran();

                return resultobj;

            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "删除失败!" + ex.toString(), Result = null };
            }
        }


        /// <summary>
        /// 批量删除商品
        /// </summary>
        /// <param name="ids">商品ids</param>
        public ResultObject deleteBatchProducts(string ids)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                List<int> listids = ids.Split(',').Select(int.Parse).ToList();
              
                bool isSuccess = Delete<Products>(listids);
                if (isSuccess)
                {
                    resultobj= new ResultObject() { Flag = 1, Message = "删除成功!", Result = ids };
                }
                else
                {
                    resultobj= new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }


                _db.Ado.CommitTran();

                return resultobj;


            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "删除失败!" + ex.toString(), Result = null };
            }
        }

        public Products getProductsById(int id)
        {
            var outobj = GetById<Products>(id);
            //查找规格
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
              new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

            var productSpecs = GetList<ProductSpecs>(it => it.ProductId == id, orderbyList);
            outobj.ProductSpecs = productSpecs;
            return outobj;
        }









        /// <summary>
        /// 修改规格图片
        /// </summary>
        /// <param name="ProductSpecs">规格cVO</param>

        public ResultObject updateProductSpecsImage(ProductSpecs cVO, string[] updateColums = null)
        {
    
            int specId = cVO.SpecId;
  

            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj=null;

                if (specId != 0)
                {

                    bool isSuccess = Update<ProductSpecs>(cVO, updateColums);
                    if (isSuccess)
                        resultobj = new ResultObject() { Flag = 1, Message = "更新成功!", Result = cVO };
                    else
                        resultobj = new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }

                _db.Ado.CommitTran();

                return resultobj;
            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }













        /// <summary>
        /// 删除商品规格
        /// </summary>
        /// <param name="id">规格id</param>
        public ResultObject deleteProductSpecs(int id)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;

                bool isSuccess = Delete<ProductSpecs>(id);
                if (isSuccess)
                {
                    resultobj= new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
                }
                else
                {
                    resultobj= new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }

                _db.Ado.CommitTran();

                return resultobj;

            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();

                return new ResultObject() { Flag = 0, Message = "删除失败!" + ex.toString(), Result = null };
            }
        }



        /// <summary>
        /// 批量删除商品规格
        /// </summary>
        /// <param name="ids">规格ids</param>
        public ResultObject  deleteBatchProductSpecs(string ids)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                List<int> listids = ids.Split(',').Select(int.Parse).ToList();

                bool isSuccess = Delete<ProductSpecs>(listids);
                if (isSuccess)
                {
                    resultobj = new ResultObject() { Flag = 1, Message = "删除成功!", Result = ids };
                }
                else
                {
                    resultobj = new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }


                _db.Ado.CommitTran();

                return resultobj;


            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "删除失败!" + ex.toString(), Result = null };
            }
        }


        public List<Categories> getCategoriesOptions(int appType,int businessId)
        {
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
              new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

            //加查询条件
            var conModels = new List<IConditionalModel>();
            conModels.add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });
            conModels.add(new ConditionalModel { FieldName = "BusinessId", ConditionalType = ConditionalType.Equal, FieldValue = businessId.toString() });


            conModels.add(new ConditionalModel { FieldName = "Status", ConditionalType = ConditionalType.Equal, FieldValue = "1" });


            //查询树状结构
            var objlist = _db.Queryable<Categories>().Where(conModels).OrderBy(orderbyList);

            var list = objlist.ToTree(u => u.Children, u => u.ParentId, 0, it => it.CategoryId);
       
    
            return list;

        }

    }



}

