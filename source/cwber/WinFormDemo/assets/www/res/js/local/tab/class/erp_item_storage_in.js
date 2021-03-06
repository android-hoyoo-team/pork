var erp_item_storage_in=Class({
	
		storage_item_grid:null,
		item_info:null,
		storage_add:null,
		div:null,
		_id:{index:0},
		param:{
			count:null,
			div:'',
			item_number:'',
			feeding_id:null,
			name:'',
			ok:null,
			cancle:null
		},
		initialize:function()
		{
			if(!arguments[0])
			{
				tipOnce('错误','请刷新重试。');
				throw new Error("erp_item_storage_in参数错误。");
			}
			
			if(arguments[0])
			{
				/**
				 * if() 参数判断
				 * */
				this.param=Util.extend(this.param,arguments[0]);
			}
			this._id.index++;
			this.id=this._id.index;
			this.name='__erp_item_storage_in'+this.id;
			window[this.name]=this;
			this.param.div='#'+this.param.div.replace(/^#/,'');
			$(this.param.div).empty();
			this.init_item_info();
			this.init_item_storage();
			//arguments;
		},
		init_item_info:function()
		{
			//var item_info =$("<div id='item_info'></div>");
			$(this.param.div).append($("<div id='item_info'></div>"));
			this.item_info=new erp_item_info({
				div:'item_info',
				item_number:this.param.item_number
			});
		},
		init_item_storage:function()
		{
			var target=this;
			//$(this.param.grid_div).empty();
			var add_storage =$("<div id='add_storage'></div>");
			$(this.param.div).append(add_storage);
			var storage_in_grid =$("<div id='storage_in_grid'></div>");
			$(this.param.div).append(storage_in_grid);
			storage_in_grid.unbind().bind('mouseover',function(){
				if(!target.bind_mouseover_tag)
				{
					for(var i=0;i<target.storage_item_grid.options.pageSize;i++)
					{
						if($("#add_count"+i).length>0)
						{
//						console_info(i);
							$("#add_count"+i).unbind().bind('click',function(index){
								return function(){
//								console_info(target.param.count);
									if(target.param.count)
										$("#add_count"+index).val(target.param.count);
									//$("#add_count"+index).unbind();
								};
							}(i));
						}
					}
					target.bind_mouseover_tag=1;
//						console_info('11');
				}
			});
			this.storage_item_grid=storage_in_grid.ligerGrid(ERPUtils.get_default_grid_option_for_url(
						{url:erp_api_service.EntRelStorageItem[0]+"?item_number="+this.param.item_number+"&actual_count__isnull=false&future_count__isnull=true",
							width:"95%",
							height:300,
							isScoll:true,
							columns: [
								{
									display : '库存',
									type:'int',
									width:70,
									name : 'actual_count'
								},
								/*{
									display : '预期库存',
									//width:80,
									name : 'future_count'
								},*/
								{
									display : '状态',
									name : 'type',
									width:70,
									 editor: { type: 'select',emptyText: null, data:getStorageItemTypeArry(), valueColumnName: 'type', displayColumnName: 'name' },
									 render: function (rowdata, rowindex, value) {
										 return getStorageItemTypeMap()[value];
									 }
								},
								{
									display : '类型信息',
									name : 'type_info',
									width:70,
									editor: { type: 'text'}
								},
								{                         
									display : '仓库位置',
									name : 'storage_name',
									type:'int',
									width:80,
									editor:{type:'text'}
									/*editor:{ 	
										type: 'select',
			                  	  		emptyvalue:false, 
			                  	  		data:storage_data, 
			                  	  		valueColumnName: 'id', 
			                  	  		displayColumnName: 'name',
			                  	  		selectBoxWidth: 300 ,
			                  	  		selectBoxHeight:300,
			                  	  		//isShowCheckBox:false,
			                  	  		emptyText: null,
			                  	  		//emptyValue:0,
			                  	  		valueType:"int",
			                  	  		columns: [
			                                            //{ header: 'ID', name: 'id',type:'int', width: 20 },
			                                            { header: '名称', name: 'name' }
			                                        ]
			                	},
			                    render:function (item)
			                    {
			                    			for (var i = 0; i < storage_data.length; i++)
			                    			{
			                    					if (storage_data[i]['id']+"" == item.storage_id+"")
			                    					{
			                    						return storage_data[i]['name'];
			                    					}
			                    			}
			                    			return "";
			                    }*/
								},
								{ display: '增加库存数量', name: 'add_count',
									width:100,
									editor: { type: 'int'},
									render: function (rowdata, rowindex, value) {
			                  		  return "<input id='add_count"+rowindex+"' type='text' style='width:80px'/>";
			                  	  }
			                    },
								{ display: '入库备注', name: 'add_remark', align: 'right', 
			                    	width:150,
									editor: { type: 'text'},
									render: function (rowdata, rowindex, value) {
				                  		  return "<input id='add_remark"+rowindex+"' type='text'style='width:130px'/>";
				                  	  }
			                    },
			                    {
			                    	width:80,
			                        display: '操作', isSort: false, filter:false, render: function (rowdata, rowindex, value) {
			                            var h = "";
			                            if (!rowdata._editing) {
			                                h += "<a id='"+target.name+"_addCount_"+rowindex+"' href='javascript:"+target.name+".addCount(" + rowindex + ")'>入库</a> ";
//			                                console_info(rowdata);
			                                if(rowdata.actual_count==null||rowdata.actual_count+''=='0')
			                                	h += "<a href='javascript:"+target.name+".delete_storage(" + rowindex + ")'>删除</a> ";
			                            }
			                            else {
			                                h += "<a href='javascript:"+target.name+".endEdit(" + rowindex + ")'>提交</a> ";
			                                h += "<a href='javascript:"+target.name+".cancelEdit(" + rowindex + ")'>取消</a> ";
			                            }
			                            return h;
			                        }
			                    }
								],
								 onBeforeSubmitEdit:function()
			                     {
			                         var param;
			                         if (arguments[0].record.__status == "add") {
			                        	 //alert(arguments[0].newdata.__index).val());
			                        	 var r= /^[0-9]*[1-9][0-9]*$/ ;
			                        	 var add;
			                        		if(r.test(arguments[0].newdata.add_count)){
			                        			add=arguments[0].newdata.add_count;
			                        		}
			                        		else{
			                        			tipOnce("提示", target.param.item_number+"入库数量不准确", 5000);
			                        			return false;
			                        		}
			                        		var inrecord_data={
			                        					"count": add,
			                        		           "item_number": target.param.item_number,
			                        		           'feeding_id': target.param.feeding_id,
			                        		           "item_type": arguments[0].newdata.type,
			                        		           "remark": arguments[0].newdata.add_remark,
			                        		           "storage_id": arguments[0].newdata.storage_id,
			                        		           "type": "入库"
			                        		           };
			                        		target.add_info_to_iorecord(inrecord_data);
			                             param = {
			                                 url: erp_api_service.EntRelStorageItem[0],
			                                 method: "POST",
			                                 data: {"actual_count":add,
			                                	 "item_number":target.param.item_number, "storage_id":arguments[0].newdata.storage_id,
			                                	 "type":arguments[0].newdata.type
			                                 }
			                                 //encode:"utf-8"
			                             };
			                         }
			                         else {
			                             param = {
			                                 url: erp_api_service.EntRelStorageItem[0] + arguments[0].record.id,
			                                 method: "PATCH",
			                                 //data: Util.extend(arguments[0].record, arguments[0].newdata)
			                                 data:arguments[0].newdata
			                                 //encode:"utf-8"
			                             };
			                         }
			                          var res = JSON.parse(bridge_map.ajax(JSON.stringify(param)));
			                         if (res != null && res.status > 199 && res.status<300) {
			                             tipOnce("提示", target.param.item_number+"入库成功", 5000);
			                             target.storage_item_grid.reload();
			                             target.item_info.refresh();
			                             return true;
			                         }
			                         else {
			                             tipOnce("提示", Util.formatString(target.param.item_number+"修改失败，请重试[失败码:{0}]", res?res.status:null), 10000)
			                             return false;
			                         }
			                     },
								 toolbar: { items: [
								                    
								                    	{
								                        text: '添加新库位', click: function () {
								                        	if(!target.storage_add)
								                        		target.storage_add =new erp_storage_add({div:'add_storage',item_number:target.param.item_number,
									                        		success:function(){
									                        			target.storage_item_grid.reload();
									                        			target.item_info.refresh();
									                        		},error:function(){
									                        			
									                        		}
								                        		});
								                        	else
								                        	{
								                        		target.storage_add.toggle();
								                        	}
								                        }, icon: 'add'
								                    	}
								                    ]
								 	},
								 	enabledEdit: true, 
								 	checkbox:false,
								 	clickToEdit: false,
								 	rownumbers:true
						})
				);
		},
		delete_storage:function(rowid)
		{
			var row=this.storage_item_grid.getRow(rowid);
			if(row.id)
			{
				if(confirm("确定删除?"))
				{
					if(ERPUtils.deleteEntityById(row.id,erp_api_service.EntRelStorageItem[0]))
					{
						tipOnce("提示", "删除成功", 5000);
						this.storage_item_grid.reload();
						this.item_info.refresh();
					}
					else
					{
						tipOnce("提示", "删除失败,请刷新后重试", 5000);
					}
				}
			}
			
		},
		 addCount:function(rowid) {
			var row=this.storage_item_grid.getRow(rowid);
			var old=Number(row.actual_count);
			var string_add=$("#add_count"+row.__index).val();
			var string_remark=$("#add_remark"+row.__index).val();
			var add;
			/*验证 是否合法
			 * */
			var r= /^[0-9]*[1-9][0-9]*$/ ;
			if(r.test(string_add)){
				add=Number(string_add);
			}
			else{
				tipOnce("提示", "入库数量不准确", 5000);
				return;
			}
			var inrecord_data={"count": add,
						'feeding_id': this.param.feeding_id,
			           "item_number": this.param.item_number,
			           "item_type": row.type,
			           "remark": string_remark,
			           "storage_id": row.storage_id,
			           "type": "入库"
			           };
			var target=this;
			 var param = {
		             url: erp_api_service.EntRelStorageItem[0] + row.id,
		             method: "PATCH",
		             //data: Util.extend(arguments[0].record, arguments[0].newdata)
		             data:{actual_count:old+add},
		             callBack:function(r){
			            	var res=JSON.parse(r);
			    		    if (res != null && res.status > 199 && res.status<300) {
			    		    	target.add_info_to_iorecord(inrecord_data);
			    		         tipOnce("提示", target.param.item_number+"入库成功", 10000);
			    		        if(typeof (target.param.success) =='function')
			    		        {
			    		        	target.param.success();
			    		        }
			    		        target.storage_item_grid.reload();
			    		        target.item_info.refresh();
			    		     }
			    		     else
			    		    {
			    		    	 if(typeof (target.param.error) =='function')
			    		    	 {
			    		    		 target.param.error();
			    		    	 }
			    		    	 tipOnce("提示", target.param.item_number+"入库失败", 10000);
			    		    	 target.storage_item_grid.reload();
			    		    }
			             }
		             //encode:"utf-8"
		         };
			 $("#"+this.name+"_addCount_"+rowid).html("入库中...");
			 $("#"+this.name+"_addCount_"+rowid).attr("href","javascript:void(0);");
			 bridge_map.ajax_1_auto(param);
		},
		cancelEdit:function (rowid) {
			this.storage_item_grid.cancelEdit(rowid);
			this.storage_item_grid.deleteRow(rowid);
		},
		endEdit:function (rowid) {
			this.storage_item_grid.endEdit(rowid);
		},
		add_info_to_iorecord:function (data)
		{
			var target=this;
			var param = {
		            url: erp_api_service.EntStorageChangesRecord[0],
		            method: "POST",
		            data: data,
		            callBack:function(r){
		            	var res=JSON.parse(r);
		            	if (res != null && res.status > 199 && res.status<300) {
		            		target.storage_item_grid.reload();
		  		          if(target.param.feeding_id)
		  		          {
		  		        	  var result= bridge_map.super_api(Util.formatString(erp_api_sql.p_feeding_into_stroage,target.param.feeding_id));
		  		        	  if(result != null && result.status > 199 && result.status<300)
		  		        	  {
		  		        		  tipOnce("提示",target.param.item_number+"预入库修改成功", 10000);
		  		        	  }
		  		        	  else{
		  		        		  tipOnce("提示",target.param.item_number+"预入库修改失败", 15000)
		  		        	  }
		  		        	  target.item_info.refresh();
		  		          }
		  		          tipOnce("提示",target.param.item_number+"入库记录添加成功",10000);
		  		          return true;
		  		      }
		  		      else {
		  		    	  target.add_info_to_iorecord_try_time=target.add_info_to_iorecord_try_time?1:target.add_info_to_iorecord_try_time+1;
		  		          tipOnce("提示", Util.formatString(target.param.item_number+"入库记录添加失败[失败码:{0}],正在重试第"+target.add_info_to_iorecord_try_time+"次", res?res.status:null), 10000);
		  		          if(target.add_info_to_iorecord_try_time==6)
		  		        	  tipOnce("提示",target.param.item_number+"已经尝试6次依然失败，只好放弃");  
		  		          else
		  		        	  target.add_info_to_iorecord(data);
		  		          return false;
		  		      }
		            }
		      };
			  bridge_map.ajax_1_auto(param);
		}
});
